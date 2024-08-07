using InitDataBaseTool.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitDataBaseTool.Events
{
    public class DataBaseEvent:PubSubEvent<DataBaseManager>
    {
    }
}
