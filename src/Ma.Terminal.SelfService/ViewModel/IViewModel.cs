using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.ViewModel
{
    public interface IViewModel
    {
        string Title { get; set; }

        bool IsShown { get; set; }
    }
}
