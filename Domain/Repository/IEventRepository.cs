using Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IEventRepository:Common.Repository.IEventRepository
    {
        Task SaveEventAsync(EventModel @event);
    }
}
