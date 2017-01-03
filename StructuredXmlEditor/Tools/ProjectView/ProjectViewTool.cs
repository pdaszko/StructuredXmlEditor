﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructuredXmlEditor.Data;
using System.Windows;

namespace StructuredXmlEditor.Tools
{
	public class ProjectViewTool : ToolBase
	{
		public static ProjectViewTool Instance;

		public ProjectItem Root { get; private set; }
		public IEnumerable<ProjectItem> Items
		{
			get
			{
				if (Root != null)
				{
					foreach (var item in Root.Items) yield return item;
				}
			}
		}

		public ProjectViewTool(Workspace workspace) : base(workspace, "ProjectView")
		{
			Instance = this;
			Reload();
		}

		public void Reload()
		{
			Task.Run(() =>
			{
				Root = new ProjectItem(Workspace, null, this, "");
				Root.IsExpanded = true;
				DeferredRefresh();
			});
		}

		public void DeferredRefresh()
		{
			Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() => 
			{
				RaisePropertyChangedEvent("Items");
			}));
		}
	}
}