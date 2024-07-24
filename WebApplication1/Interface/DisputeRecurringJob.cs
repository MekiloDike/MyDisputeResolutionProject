using Azure.Core;
using DisputeResolutionCore.Enum;
using DisputeResolutionCore.Interface;
using DisputeResolutionInfrastructure.Context;
using DisputeResolutionInfrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace DisputeResolutionBackgroundService.Interface
{
    public class DisputeRecurringJob : IDisputeRecurringJob
    {
        private readonly DisputeContext _dbContext;
        private readonly ILogTransaction _logTransaction;
        private readonly IDispute _dispute;

        public DisputeRecurringJob(DisputeContext disputeContext, ILogTransaction logTransaction, IDispute dispute)
        {
            _dbContext = disputeContext;
            _logTransaction = logTransaction;
            _dispute = dispute;
        }


        public async Task GetCreatedDisputeResponseJob()
        {
            //get a list of all DisputeResponseLog records where status = pending
            var getLogResponse = _dbContext.DisputeResponseLogs.Where(x => x.status == Status.PENDING.ToString() && !string.IsNullOrEmpty(x.logCode)).ToList();
            //if null, return
            if (getLogResponse == null)
            {
                return;
            }
            //if not null loop through them and call the get interswitch
            foreach (var item in getLogResponse)
            {
                var disputeResponse = await _dispute.GetDispute(item.logCode);
                //check if the response is null,if null continue
                if (disputeResponse == null)
                {
                    continue;
                }
                else
                {
                    var journalList = new List<Journal>();
                    foreach (var resp in disputeResponse.journal)
                    {
                        var journal = new Journal
                        {
                            disputeId = resp.disputeId,
                            addedBy = resp.addedBy,
                            addedOn = resp.addedOn,
                            detail = resp.detail,
                            DisputeResponseLogId = item.Id,
                            DisputeResponseLog = item
                        };                      
                    journalList.Add(journal);
                    }

                        var evidenceList = new List<Evidence>();
                        foreach (var res in disputeResponse.evidence)
                        {
                            var evidence = new Evidence
                            {
                                disputeId = res.disputeId,
                                base64EncodedBinary = res.base64EncodedBinary,
                                mimeType = res.mimeType,
                                tags = res.tags,
                                uuId = res.uuId,
                                DisputeResponseLogId = item.Id,
                                DisputeResponseLog = item
                                
                            };
                        evidenceList.Add(evidence);
                        }

                    //update the table and save
                    item.issuer = disputeResponse.issuer;
                    item.pan = disputeResponse.pan;
                    item.journal = journalList;
                    item.evidence = evidenceList; 
                   item.statusActions = disputeResponse.statusActions.ToString();
                _dbContext.DisputeResponseLogs.Update(item);
                }
               await _dbContext.SaveChangesAsync();
            }
            return;
        }

        public async Task CreateDisputeJob()
        {
            //Get the logged request from the DisputeResquestLog table where isDisputeCreated = false (a list)
            var loggedRequest = _dbContext.DisputeRequestLogs.Where(x => x.IsDisputeCreated == false).ToList();
            //if null, return 
            if (loggedRequest == null)
            {
                return;
            }
            //if not null,loop through each request and call the method that start interswitch: StartInterswitchCall

            foreach (var item in loggedRequest)
            {
                var interswitchCall = await _logTransaction.StartInterswitchCall(item);
                //if false, continue
                if (interswitchCall.isCreated == false)
                {
                    continue;
                }
                else
                {
                    //else, update DisputeResquestLog table set isDisputeCreated = true and savechanges
                    item.IsDisputeCreated = true;
                    _dbContext.DisputeRequestLogs.Update(item);
                    await _dbContext.SaveChangesAsync();
                    //and create a new DisputeResponseLog record add the logcode, transactionLogRef, status = Pending and save
                    var disputeResponselog = new DisputeResponseLog
                    {
                        logCode = interswitchCall.logcode,
                        status = Status.PENDING.ToString(),
                        transactionLogReference = item.TransactionLogRefernce,
                    };
                    _dbContext.Add(disputeResponselog);
                    _dbContext.SaveChanges();
                }
            }
            return;
        }
    }
}
