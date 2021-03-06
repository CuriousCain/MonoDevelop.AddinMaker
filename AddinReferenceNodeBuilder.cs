﻿using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Components;

namespace MonoDevelop.AddinMaker
{
	class AddinReferenceNodeBuilder : TypeNodeBuilder
	{
		public override Type NodeDataType 
		{
			get { return typeof (AddinReference); }
		}

		public override string GetNodeName (ITreeNavigator thisNode, object dataObject)
		{
			return "AddinReference";
		}

		public override string ContextMenuAddinPath {
			get {
				return "/MonoDevelop/AddinMaker/ContextMenu/ProjectPad/AddinReference";
			}
		}

		public override Type CommandHandlerType
		{
			get { return typeof (AddinReferenceCommandHandler); }
		}

		public override object GetParentObject (object dataObject)
		{
			var addin = (AddinReference)dataObject;
			return addin.OwnerProject != null ? addin.OwnerProject.AddinReferences : null;
		}

		public override void BuildNode (ITreeBuilder treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			var addin = (AddinReference)dataObject;

			nodeInfo.Label = GLib.Markup.EscapeText (addin.Id);

			//TODO: custom icon
			nodeInfo.Icon = Context.GetIcon ("md-reference-package");

			//TODO: get state, mark if unresolved
			//nodeInfo.StatusSeverity = TaskSeverity.Error;
			//nodeInfo.StatusMessage = GettextCatalog.GetString ("Could not resolve addin");
		}

		public override bool HasChildNodes (ITreeBuilder builder, object dataObject)
		{
			return false;
		}

		class AddinReferenceCommandHandler : NodeCommandHandler
		{
			public override bool CanDeleteItem ()
			{
				return true;
			}

			[CommandUpdateHandler (MonoDevelop.Ide.Commands.EditCommands.Delete)]
			public void UpdateDelete (CommandInfo info)
			{
				info.Enabled = true;
				info.Text = GettextCatalog.GetString ("Remove");
			}

			[CommandHandler (MonoDevelop.Ide.Commands.EditCommands.Delete)]
			public override void DeleteItem ()
			{
				var addin = (AddinReference) CurrentNode.DataItem;
				addin.OwnerProject.AddinReferences.Remove (addin);
			}
		}
	}
}