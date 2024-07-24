using DisputeResolutionInfrastructure.Entity;

namespace DisputeResolutionBackgroundService.Interface
{
    public interface IDisputeRecurringJob
    {
        //getting all failed dispute and retrying to create dispute. if successfully, update the table
       Task CreateDisputeJob();

        //get all dispute created request and call interswitch getDispute. if successfull, update the table accordingly
       Task GetCreatedDisputeResponseJob();

        
    }
}
