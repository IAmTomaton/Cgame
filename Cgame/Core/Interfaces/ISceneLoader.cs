using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame
{
    public interface ISceneLoader
    {
        void LoadNextScene();
        void Update();
    }
}
