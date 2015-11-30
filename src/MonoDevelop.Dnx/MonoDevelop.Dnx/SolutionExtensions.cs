﻿//
// SolutionExtensions.cs
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

using System.IO;
using System.Collections.Generic;
using System.Linq;
using MonoDevelop.Core;
using MonoDevelop.Projects;

namespace MonoDevelop.Dnx
{
	public static class SolutionExtensions
	{
		public static bool HasDnxProjects (this Solution solution)
		{
			return solution.GetDnxProjects ().Any ();
		}

		public static IEnumerable<DnxProject> GetDnxProjects (this Solution solution)
		{
			return solution.GetAllSolutionItems<DnxProject> ();
		}

		public static DnxProject FindProjectByProjectJsonFileName (this Solution solution, string fileName)
		{
			var directory = new FilePath (Path.GetDirectoryName(fileName));
			return solution.GetAllSolutionItems<DnxProject> ()
				.FirstOrDefault (project => project.BaseDirectory == directory);
		}

		public static SolutionFolder AddSolutionFolder (this Solution solution, string name, params FilePath[] files)
		{
			var solutionFolder = new SolutionFolder {
				Name = name
			};

			foreach (FilePath file in files) {
				solutionFolder.Files.Add (file);
			}

			solution.RootFolder.AddItem (solutionFolder);
			return solutionFolder;
		}

		public static void GenerateDefaultDnxProjectConfigurations (this Solution solution, DnxProject project)
		{
			foreach (SolutionItemConfiguration configuration in project.Configurations) {
				SolutionConfiguration existingConfiguration = solution.GetConfiguration (configuration);
				if (existingConfiguration == null) {
					SolutionConfiguration newConfiguration = solution.AddConfiguration (configuration.Name, false);
					newConfiguration.AddItem (project);
				}
			}
		}

		static SolutionConfiguration GetConfiguration (this Solution solution, SolutionItemConfiguration configuration)
		{
			foreach (SolutionConfiguration existingConfiguration in solution.Configurations) {
				if (existingConfiguration.Id == configuration.Id)
					return existingConfiguration;
			}
			return null;
		}

		public static void EnsureConfigurationHasBuildEnabled (this Solution solution, DnxProject project)
		{
			foreach (SolutionConfiguration solutionConfiguration in solution.Configurations) {
				foreach (SolutionConfigurationEntry projectConfiguration in solutionConfiguration.Configurations) {
					if (projectConfiguration.Item == project && !projectConfiguration.Build) {
						projectConfiguration.Build = true;
					}
				}
			}
		}
	}
}

