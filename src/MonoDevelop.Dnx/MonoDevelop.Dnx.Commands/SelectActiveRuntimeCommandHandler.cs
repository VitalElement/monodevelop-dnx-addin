﻿//
// SelectActiveRuntimeHandler.cs
//
// Author:
//       Matt Ward <ward.matt@gmail.com>
//
// Copyright (c) 2015 Matthew Ward
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using OmniSharp.Models;

namespace MonoDevelop.Dnx.Commands
{
	public class SelectActiveRuntimeCommandHandler : CommandHandler
	{
		protected override void Update (CommandArrayInfo info)
		{
			var project = IdeApp.ProjectOperations.CurrentSelectedProject as XProject;
			if (project == null) {
				info.Bypass = true;
				return;
			}

			var dnxProject = project.AsFlavor<DnxProject> ();
			foreach (DnxFramework framework in dnxProject.GetFrameworks ()) {
				CommandInfo item = info.Add (framework.FriendlyName, framework);
				if (framework.Name == dnxProject.CurrentFramework) {
					item.Checked = true;
				}
			}
		}

		protected override void Run (object dataItem)
		{
			var project = IdeApp.ProjectOperations.CurrentSelectedProject as XProject;
			if (project != null) {
				var dnxProject = project.AsFlavor<DnxProject> ();
				dnxProject.UpdateReferences ((DnxFramework)dataItem);
			}
		}
	}
}

