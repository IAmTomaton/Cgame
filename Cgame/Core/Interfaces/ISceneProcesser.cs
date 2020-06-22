using Cgame.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame
{
    public interface ISceneProcesser
    {
        (bool ifAdded, GameObject newObject) Process(string command);
    }
}
