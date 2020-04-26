using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WakeTFT
{
    class Engine
    {
        Driver driver = new Driver();
        GameInstance Game = new GameInstance();
        public Engine()
        {
            driver.connect();
        }


        public void Run()
        {
            while (true)
            {
                if (Utility.IsOpen)
                {
                    if (Game.ProcessId == Utility.LeagueId)
                    {

                    }
                    else
                    {
                        Game.Address = driver.get_process_base_address(Utility.LeagueId);
                        Game.Time = driver.read<float>(Utility.LeagueId, Game.Address + (UInt64)GameOffsets.GamePointer.oGameTime);
                        Game.Renderer = driver.read<UIntPtr>(Utility.LeagueId, Game.Address + (UInt64)GameOffsets.GamePointer.oRenderer);
                        Game.ProcessId = Utility.LeagueId;
                    }


                }
                else
                {

                }
                Thread.Sleep(2000);
            }
        }

    }
}
