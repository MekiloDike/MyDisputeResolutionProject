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
        public async Task<GenericResponse<bool>> CreateTransactionLog(DisputeRequestLogDto Request)
        {

            var transactionType = GetTransactionType(Request.TransactionLogRefernce);
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
                Stan = Request.Stan,
                MaskCardPan = Request.MaskCardPan,
                TerminalId = Request.TerminalId,
                TransactionDate = Request.TransactionDate,
                TransactionLogRefernce = Request.TransactionLogRefernce,
                RetrivalNumber = Request.RetrivalNumber,
                Amount = Request.Amount,
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

            //call get transaction
            if (transactionType == TransactionType.AgencyBanking)
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
                    return result;
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

                if(string.IsNullOrEmpty(logCode))
                {
                    return result;
                }
                //if successful update the request log table, isDisputeCreated = true
                disputeRequestLog.IsDisputeCreated = true;
                _dbContext.DisputeRequestLogs.Update(disputeRequestLog);
                await _dbContext.SaveChangesAsync();

                //save the logcode to the response log with status = Pending
                var disputeResponse = new DisputeResponseLog
                {
                    logCode = logCode,
                    status = "PENDING",
                    transactionLogReference = Request.TransactionLogRefernce,
                };
                await _dbContext.AddAsync(disputeResponse);
                await _dbContext.SaveChangesAsync();
            }

            if (transactionType == TransactionType.IpgTransaction)
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
                    return result;
                }
                var createDisputeRequest = new CreateDisputeRequest
                {
                    comment = "",
                    reasonCode = "RG",
                    transactionReference = "",
                    disputeAmount = Request.Amount,
                    disputeAmountType = "Full",
                    category = "Chargeback",
                    transactionType = "",
                }
                var logcode = await _Dispute.CreateDispute(createDisputeRequest);
                if (string.IsNullOrEmpty(logcode))
                {
                    return result;
                }
                
                disputeRequestLog.IsDisputeCreated = true;
                _dbContext.DisputeRequestLogs.Update(disputeRequestLog);
                await _dbContext.SaveChangesAsync();

                //save the logcode to the response log with status = Pending
                var disputeResponse = new DisputeResponseLog
                {
                    logCode = logcode,
                    status = "PENDING",
                    transactionLogReference = Request.TransactionLogRefernce,
                };
                await _dbContext.AddAsync(disputeResponse);
                await _dbContext.SaveChangesAsync();
            }

            if (transactionType == TransactionType.TransferTransaction)
            {
                var transferTransaction = new TransferTransactionRequest
                {
                   date = DateTime.Now,
                   pan = Request.MaskCardPan,
                   stan = Request.Stan,
                   terminal = Request.TerminalId,
                };
                var transferResponse = _transaction.GetTransferTransaction(transferTransaction);
                if (transferResponse == null)
                {
                    return result;
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
                    return result;
                }

                disputeRequestLog.IsDisputeCreated = true;
                _dbContext.DisputeRequestLogs.Update(disputeRequestLog);
                await _dbContext.SaveChangesAsync();

                //save the logcode to the response log with status = Pending
                var disputeResponse = new DisputeResponseLog
                {
                    logCode = logcode,
                    status = "PENDING",
                    transactionLogReference = Request.TransactionLogRefernce,
                };
                await _dbContext.AddAsync(disputeResponse);
                await _dbContext.SaveChangesAsync();
            }

            return result;
        }

        public async Task<GenericResponse<DisputeResponseLogDto>> GetLoggedTransaction(string transactionReference)
        {
            var getResponse = await _dbContext.DisputeResponseLogs
                .Include(x => x.evidence)
                .Include(x => x.journal)
                .FirstOrDefaultAsync(x => x.transactionLogReference == transactionReference);

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
            var evidencelist = new List<EvidenceDto>();
            foreach (var item in getResponse.evidence)
            {
                var evidenceDto = new EvidenceDto
                {
                    base64EncodedBinary = item.base64EncodedBinary,
                    disputeId = item.disputeId,
                    uuId = item.uuId,
                    mimeType = item.mimeType,
                    tags = item.tags,
                };
                evidencelist.Add(evidenceDto);
            }

            // map DisputeRequestLog Entity to DisputeRequestLogDto
            var disputeResponseLogDto = new DisputeResponseLogDto
            {
                evidence = evidencelist,
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

    }
}
