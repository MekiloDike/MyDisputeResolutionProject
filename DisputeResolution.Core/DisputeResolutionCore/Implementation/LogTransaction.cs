using Azure.Core;
using DisputeResolutionCore.Dto;
using DisputeResolutionCore.Enum;
using DisputeResolutionCore.Interface;
using DisputeResolutionInfrastructure.Context;
using DisputeResolutionInfrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisputeResolutionCore.Implementation
{
    public class LogTransaction : ILogTransaction
    {
        private readonly DisputeContext _dbContext;
        private readonly ITransaction _transaction;
        private readonly IDispute _Dispute;
        public LogTransaction(DisputeContext dbContext, ITransaction transaction, IDispute dispute)
        {
            _dbContext = dbContext;
            _transaction = transaction;
            _Dispute = dispute;
        }
        public async Task<GenericResponse<bool>> CreateTransactionLog(DisputeRequestLogDto request)
        {
            // check if there is an existing TransactionLogRefernce in the database table and return transaction with TransactionLogRefernce already logged
            var loggedRequest = _dbContext.DisputeRequestLogs.FirstOrDefault( x => x.TransactionLogRefernce == request.TransactionLogRefernce);
            if (loggedRequest != null)
            {
                return new GenericResponse<bool>
                {
                    IsSuccessful = false,
                    Message = "TransactionLogRefernce already exist"
                }; 
            }

            var transactionType = GetTransactionType(request.TransactionLogRefernce);
            if (transactionType == TransactionType.Invalid)
            {
                return new GenericResponse<bool>
                {
                    IsSuccessful = false,
                    Message = "invalid Transaction Type",
                };
            }

            // map DisputeRequestLogDto to DisputeRequestLog Entity
            var disputeRequestLog = new DisputeResquestLog
            {
                Stan = request.Stan, 
                MaskCardPan = request.MaskCardPan,
                TerminalId = request.TerminalId,
                TransactionDate = request.TransactionDate,
                TransactionLogRefernce = request.TransactionLogRefernce,
                RetrivalNumber = request.RetrivalNumber,
                Amount = request.Amount,
                TransactionType = transactionType.ToString(),
                IsDisputeCreated = false
            };
           
            await _dbContext.DisputeRequestLogs.AddAsync(disputeRequestLog);
            var saved = _dbContext.SaveChanges() > 0;

            var result = new GenericResponse<bool>
            {
                IsSuccessful = saved,
                Message = saved == true ? "Successfully Logged request" : "Failed to Log request",

            };

            return result;
        }

        public async Task<GenericResponse<DisputeResponseLogDto>> GetLoggedTransaction(string transactionLogReference)
        {
            var getResponse = await _dbContext.DisputeResponseLogs
                .Include(x => x.evidence)
                .Include(x => x.journal)
                .FirstOrDefaultAsync(x => x.transactionLogReference == transactionLogReference);

            if (getResponse == null)
            {
                return new GenericResponse<DisputeResponseLogDto>
                {
                    IsSuccessful = true,
                    Message = "No record found",
                    Data = null
                };
            }

            var journalList = new List<JournalDto>();
            foreach (var item in getResponse.journal)
            {
                var journalDto = new JournalDto
                {
                    disputeId = item.disputeId,
                    addedBy = item.addedBy,
                    addedOn = item.addedOn,
                    detail = item.detail,
                };
                journalList.Add(journalDto);
            }
             

            // map DisputeRequestLog Entity to DisputeRequestLogDto
            var disputeResponseLogDto = new DisputeResponseLogDto
            {
                evidence = getResponse.evidence.Select(x => new EvidenceDto
                {
                    base64EncodedBinary = x.base64EncodedBinary,
                    disputeId = x.disputeId,
                    uuId = x.uuId,
                    mimeType = x.mimeType,
                    tags = x.tags,
                }).ToList(),
               
                journal = journalList,
                logCode = getResponse.logCode,
                transactionAmount = getResponse.transactionType,
                issuerCode = getResponse.issuerCode,
                issuer = getResponse.issuer,
                acquirerCode = getResponse.acquirerCode,
                acquirer = getResponse.acquirer,
                merchantCode = getResponse.merchantCode,
                merchant = getResponse.merchant,
                customerReference = getResponse.customerReference,
                transactionType = getResponse.transactionType,
                transactionCurrencyCode = getResponse.transactionCurrencyCode,
                transactionDate = getResponse.transactionDate,
                transactionLogReference = getResponse.transactionLogReference,
                transactionStore = getResponse.transactionStore,
                surchargeAmount = getResponse.surchargeAmount,
                settlementCurrencyCode = getResponse.settlementCurrencyCode,
                settlementAmount = getResponse.settlementAmount,
                terminalType = getResponse.terminalType,
                transactionReference = getResponse.transactionReference,
                disputeAmountType = getResponse.disputeAmountType,
                accountNumber = getResponse.accountNumber,
                disputeAmount = getResponse.disputeAmount,
                domainCode = getResponse.domainCode,
                merchantDisputant = getResponse.merchantDisputant,
                statusStartDate = getResponse.statusStartDate,
                status = getResponse.status,
                category = getResponse.category,
                createdBy = getResponse.createdBy,
                createdOn = getResponse.createdOn,
                reason = getResponse.reason,
                reasonCode = getResponse.reasonCode,
                region = getResponse.region,
                pan = getResponse.pan,

            };

            return new GenericResponse<DisputeResponseLogDto>
            {
                Data = disputeResponseLogDto,
                IsSuccessful = true,
                Message = ""
            };
        }

        public async Task<(bool isCreated, string logcode)> StartInterswitchCall(DisputeResquestLog Request)
        {
            //INTERSWITCH CALL
            //call get transaction
            if (Request.TransactionType == TransactionType.AgencyBanking.ToString())
            {
                var agencyBankingRequest = new AgencyBankingRequest()
                {
                    date = Request.TransactionDate,
                    pan = Request.MaskCardPan,
                    stan = Request.Stan,
                    terminal = Request.TerminalId,
                };
                //get the transaction from interswitch
                var transactionResponse = await _transaction.GetAgencyBanking(agencyBankingRequest);

                if (transactionResponse == null)
                {
                    return (false, "");
                }

                //create dispute
                var createDisputeRequest = new CreateDisputeRequest
                {
                    comment = "",
                    reasonCode = "RG",
                    transactionReference = transactionResponse.transactionReference,
                    disputeAmount = Request.Amount,
                    disputeAmountType = "Full",
                    category = "Chargeback",
                    transactionType = transactionResponse.transactionType,
                };
                var logCode = await _Dispute.CreateDispute(createDisputeRequest);

                if (string.IsNullOrEmpty(logCode))
                {
                    return (false, "");
                }  
                else
                    return (true, logCode);

            }

           else if (Request.TransactionType == TransactionType.IpgTransaction.ToString())
            {
                var ipgTransaction = new IpgTransactionRequest
                {
                    date = Request.TransactionDate,
                    maskedCardPan = Request.MaskCardPan,
                    merchantCode = "",
                    retrievalReferenceNumber = Request.RetrivalNumber,
                    stan = Request.Stan,
                };
                var ipgResponse = _transaction.GetIpgTransaction(ipgTransaction);
                if (ipgResponse == null)
                {
                    return (false, "");
                }
                var createDisputeRequest = new CreateDisputeRequest
                {
                    comment = "",
                    reasonCode = "RG",
                    transactionReference = ipgResponse.Result.transactionReference,
                    disputeAmount = Request.Amount,
                    disputeAmountType = "Full",
                    category = "Chargeback",
                    transactionType = ipgResponse.Result.transactionType,
                };
                var logcode = await _Dispute.CreateDispute(createDisputeRequest);
                if (string.IsNullOrEmpty(logcode))
                {
                    return (false, "");
                }
                else
                    return (true, logcode);
            }
            //if transaction type is transfer
            else
            {
                var transferTransaction = new TransferTransactionRequest
                {
                    date = DateTime.Now,
                    pan = Request.MaskCardPan,
                    stan = Request.Stan,
                    terminal = Request.TerminalId,
                };
                var transferResponse = _transaction.GetTransferTransaction(transferTransaction).Result;
                if (transferResponse == null)
                {
                    return (false, "");
                }
                var createDisputeRequest = new CreateDisputeRequest
                {
                    comment = "",
                    reasonCode = "RG",
                    transactionReference = transferResponse.transactionReference,
                    disputeAmount = Request.Amount,
                    disputeAmountType = "Full",
                    category = "Chargeback",
                    transactionType = transferResponse.transactionType,
                };
                var logcode = await _Dispute.CreateDispute(createDisputeRequest);
                if (string.IsNullOrEmpty(logcode))
                {
                    return (false, "");
                }

             return (true, logcode);
            }
        }

        private TransactionType GetTransactionType(string transactionInpute)
        {
            if (transactionInpute.Contains("3IPG"))
            {
                return TransactionType.IpgTransaction;
            }
            else if (transactionInpute.Contains("AFTR"))
            {
                return TransactionType.AgencyBanking;
            }
            else if (transactionInpute.Contains("WFTR"))
            {
                return TransactionType.TransferTransaction;
            }

            return TransactionType.Invalid;
        }


       /* //call the method
        var interswitchCall = await StartInterswitchCall(transactionType, request);

            //if successful re-map isDisputeCreated = true and save the first table (request)
            if (interswitchCall.isCreated)
            {
                disputeRequestLog.IsDisputeCreated = true;
                //create a new reacord to track the response from interswitch
                //save the logcode to the response log with status = Pending
                var disputeResponse = new DisputeResponseLog
                {
                    logCode = interswitchCall.logcode,
                    status = Status.PENDING.ToString(),
                    transactionLogReference = request.TransactionLogRefernce,
                };
        await _dbContext.AddAsync(disputeResponse);
        await _dbContext.SaveChangesAsync();*/
    }
}

