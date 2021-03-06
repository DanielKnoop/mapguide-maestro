﻿#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
//

#endregion Disclaimer / License

using ICSharpCode.Core;
using Maestro.Base.Services;
using Maestro.Base.UI;
using Maestro.Login;
using System.Diagnostics;

namespace Maestro.Base.Commands
{
    internal class LoginCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            RunInternal(false);
        }

        internal static void RunInternal(bool showTipOfTheDay)
        {
            using (var login = new LoginDialog())
            {
                login.Owner = Workbench.Instance;
                var result = login.ShowDialog();
                if (showTipOfTheDay)
                    TipOfTheDayDialog.FirstTimeOpen();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    var conn = login.Connection;

                    //TODO: Determine if this is a http connection. If not,
                    //wrap it in an IServerConnection decorator that will do all the
                    //request dispatch broadcasting. This will solve trac #1505 and will
                    //work for any future non-http implementations

                    var mgr = ServiceRegistry.GetService<ServerConnectionManager>();
                    Debug.Assert(mgr != null);

                    // Connection display names should be unique. A duplicate means we are connecting to the same MG server

                    LoggingService.Info("Connection created: " + conn.DisplayName); //NOXLATE
                    if (mgr.GetConnection(conn.DisplayName) == null)
                    {
                        mgr.AddConnection(conn.DisplayName, conn);
                        Workbench.Instance.ActiveSiteExplorer.FullRefresh();
                    }
                    else
                    {
                        MessageService.ShowError(Strings.ConnectionAlreadyEstablished);
                    }
                }
            }
        }
    }
}