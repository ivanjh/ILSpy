﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.Decompiler;
using ICSharpCode.NRefactory.CSharp;
using ILSpy.Debugger.AvalonEdit;
using ILSpy.Debugger.Services;
using Mono.Cecil;

namespace ILSpy.Debugger.Bookmarks
{
	public enum BreakpointAction
	{
		Break,
		Trace,
		Condition
	}
	
	public class BreakpointBookmark : MarkerBookmark
	{
		bool isHealthy = true;
		bool isEnabled = true;
		string tooltip;
		BreakpointAction action = BreakpointAction.Break;
		
		public DecompiledLanguages Language { get; private set; }
		
		public BreakpointAction Action {
			get {
				return action;
			}
			set {
				if (action != value) {
					action = value;
					Redraw();
				}
			}
		}
		
		public virtual bool IsHealthy {
			get {
				return isHealthy;
			}
			set {
				if (isHealthy != value) {
					isHealthy = value;
					Redraw();
				}
			}
		}
		
		public virtual bool IsEnabled {
			get {
				return isEnabled;
			}
			set {
				if (isEnabled != value) {
					isEnabled = value;
					if (IsEnabledChanged != null)
						IsEnabledChanged(this, EventArgs.Empty);
					Redraw();
				}
			}
		}
		
		public event EventHandler IsEnabledChanged;
		
		public string Tooltip {
			get { return tooltip; }
			set { tooltip = value; }
		}
		
		public BreakpointBookmark(TypeDefinition type, AstLocation location, BreakpointAction action, DecompiledLanguages language) : base(type, location)
		{
			this.action = action;
			this.tooltip = language.ToString();
			this.Language = language;
		}
		
		public override ImageSource Image {
			get {
				return ImageService.Breakpoint;
			}
		}
		
		public override ITextMarker CreateMarker(ITextMarkerService markerService, int offset, int length)
		{
			ITextMarker marker = markerService.Create(offset, length);
			marker.BackgroundColor = Color.FromRgb(180, 38, 38);
			marker.ForegroundColor = Colors.White;
			marker.IsVisible = b => b is MarkerBookmark && ((MarkerBookmark)b).Type == DebugData.CurrentType;
			marker.Bookmark = this;
			this.Marker = marker;
			
			return marker;
		}
	}
}