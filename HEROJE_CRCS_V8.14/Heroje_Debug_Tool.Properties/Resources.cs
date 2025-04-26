using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Heroje_Debug_Tool.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (resourceMan == null)
				{
					ResourceManager resourceManager = new ResourceManager("Heroje_Debug_Tool.Properties.Resources", typeof(Resources).Assembly);
					resourceMan = resourceManager;
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		internal static Bitmap arrow_long
		{
			get
			{
				object @object = ResourceManager.GetObject("arrow_long", resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap arrow_short
		{
			get
			{
				object @object = ResourceManager.GetObject("arrow_short", resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap bc_7
		{
			get
			{
				object @object = ResourceManager.GetObject("bc_7", resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap ImgTemp
		{
			get
			{
				object @object = ResourceManager.GetObject("ImgTemp", resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap logo
		{
			get
			{
				object @object = ResourceManager.GetObject("logo", resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Icon logoIcon
		{
			get
			{
				object @object = ResourceManager.GetObject("logoIcon", resourceCulture);
				return (Icon)@object;
			}
		}

		internal static Bitmap SaveIcon
		{
			get
			{
				object @object = ResourceManager.GetObject("SaveIcon", resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap setting_22x22
		{
			get
			{
				object @object = ResourceManager.GetObject("setting-22x22", resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap setting_26x26
		{
			get
			{
				object @object = ResourceManager.GetObject("setting-26x26", resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap setting_32x32
		{
			get
			{
				object @object = ResourceManager.GetObject("setting-32x32", resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap SoftwareStartImageA
		{
			get
			{
				object @object = ResourceManager.GetObject("SoftwareStartImageA", resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap SoftwareStartImageA2
		{
			get
			{
				object @object = ResourceManager.GetObject("SoftwareStartImageA2", resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal Resources()
		{
		}
	}
}
