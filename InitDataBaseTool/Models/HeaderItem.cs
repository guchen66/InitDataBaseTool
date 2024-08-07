using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitDataBaseTool.Models
{
    public class HeaderItem
    {
        public string HeaderName { get; set; }

        public override string ToString() => HeaderName;
    }

    public class MenuList : ObservableCollection<HeaderItem>
    {
        public MenuList()
        {
            Add(new HeaderItem() { HeaderName="连接"});
        }
    }
}
