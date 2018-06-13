using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server
{
    public class CRUD
    {
        private HelpDeskDataContext _context;

        public CRUD()
        {
            _context = new HelpDeskDataContext();
        }

        public void CreateWorkOrder(string requestorEmailAddress, DateTime dateTimeReceived, string subject, string workOrderDescription)
        {
            var appUserId = GetAppUserIdByEmail(requestorEmailAddress);
            var workOrder = new WorkOrder()
            {
                CreateDateTime = dateTimeReceived,
                ModifiedDateTime = dateTimeReceived,
                StatusDateTime = dateTimeReceived,
                RequestorId = appUserId,
                LocationId = GetLocationIdByAppUserId(appUserId),
                UnitId = GetUnitIdByAppUserId(appUserId),
                StatusId = 1,
                Subject = subject
            };

            _context.WorkOrders.InsertOnSubmit(workOrder);
            _context.SubmitChanges();
        }

        public string GetAppUserIdByEmail(string emailAddress)
        {
            return _context.AppUsers.SingleOrDefault(a => a.EmailAddress == emailAddress).Id;
        }

        public int? GetLocationIdByAppUserId(string appUserId)
        {
            return _context.AppUsers.Single(a => a.Id == appUserId).LocationId;
        }

        public int? GetUnitIdByAppUserId(string appUserId)
        {
            return _context.AppUsers.Single(a => a.Id == appUserId).UnitId;
        }
    }
}