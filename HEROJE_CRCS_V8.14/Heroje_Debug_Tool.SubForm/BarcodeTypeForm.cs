using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Heroje_Debug_Tool.BaseClass;

namespace Heroje_Debug_Tool.SubForm
{
	public class BarcodeTypeForm : Form
	{
		private SetCfgCB SetCfgCBFuncCB;

		private GetCfgCB GetCfgCBFuncCB;

		private SendCfgDataCB SendCfgDataCBFuncCB;

		private bool EnUpdateBarcodeSwitch = true;

		private ulong code_type_switch;

		private CheckBox[] BarcodeTypeCheckBox;

		private IContainer components = null;

		private GroupBox GrbOtherBarcode;

		private CheckBox ChbGridMatrix;

		private CheckBox ChbDotCode;

		private CheckBox ChbMaxicode;

		private CheckBox ChbMicroPDF417;

		private CheckBox ChbHanxin;

		private CheckBox ChbAztec;

		private GroupBox GrbUsually2D;

		private CheckBox ChbPDF417;

		private CheckBox ChbDM;

		private CheckBox ChbQRCode;

		private GroupBox GrbUsually1D;

		private CheckBox ChbCode93;

		private CheckBox ChbDataBar;

		private CheckBox ChbS25;

		private CheckBox ChbM25;

		private CheckBox ChbD25;

		private CheckBox ChbCode11;

		private CheckBox ChbI25;

		private CheckBox ChbCodaBar;

		private CheckBox ChbMSI;

		private CheckBox ChbCode39;

		private CheckBox ChbCode128;

		private CheckBox ChbUPC_EAN;

		private Button BtnOpenAllBarcodeType;

		private Button BtnOpenDefaultBarcodeType;

		private Button BtnCloseAllBarcodeType;

		private GroupBox GrbDPMSetting;

		private CheckBox ChbDM_ShortQZ;

		private CheckBox ChkDM_Not_ECC200;

		private Panel PanDmMirror;

		private Label LabDM_MirrorSet;

		private RadioButton RdbDM_MirrorAny;

		private RadioButton RdbDM_MirrorDisable;

		private RadioButton RdbDM_MirrorEnable;

		private Panel PanDmColor;

		private Label LabDM_ColorSet;

		private RadioButton RdbDM_ColorSetAny;

		private RadioButton RdbDM_ColorSetBlack;

		private RadioButton RdbDM_ColorSetWhite;

		private CheckBox ChbSmallCodeEnhance;

		private CheckBox ChbDM_SlantEnhance;

		private Panel PanDmModuleMin;

		private TextBox TxbDM_ModuleMax;

		private TextBox TxbDM_ModuleMin;

		private Button BtnDM_ModuleMax;

		private Label LabDM_ModuleMax;

		private Button BtnDM_ModuleMin;

		private Label LabDM_ModuleMin;

		private Panel PanDmModuleSizeMin;

		private TextBox TxbModuleSizeMax;

		private TextBox TxbModuleSizeMin;

		private Button BtnModuleSizeMax;

		private Label LabDMModuleSizeMax;

		private Button BtnModuleSizeMin;

		private Label LabDMModuleSizeMin;

		private Panel PanDmMaxGap;

		private Label LabDMGapMax;

		private RadioButton RdbDMGapMaxBig;

		private RadioButton RdbDMGapMaxSmall;

		private RadioButton RdbDMGapMaxNone;

		private CheckBox ChkDMDefilementEnhance;

		private CheckBox ChkDMFindEnhance;

		private Panel PanDm2dType;

		private Label LabDm2dType;

		private RadioButton RdbDMShapeAll;

		private RadioButton RdbDMShapeRectangle;

		private RadioButton RdbDMShapeSquare;

		private CheckBox ChbDMLowContrast;

		public ulong BarcodeSwitch
		{
			get
			{
				code_type_switch = 0uL;
				for (int i = 0; i < BarcodeTypeCheckBox.Length; i++)
				{
					if (BarcodeTypeCheckBox[i] != null && BarcodeTypeCheckBox[i].Checked)
					{
						code_type_switch |= (ulong)(1L << i);
					}
				}
				return code_type_switch;
			}
			set
			{
				code_type_switch = value;
				for (int i = 0; i < BarcodeTypeCheckBox.Length; i++)
				{
					if (BarcodeTypeCheckBox[i] != null)
					{
						BarcodeTypeCheckBox[i].Checked = (code_type_switch & (ulong)(1L << i)) != 0;
					}
				}
			}
		}

		public BarcodeTypeForm()
		{
			InitializeComponent();
		}

		private void BarcodeTypeForm_Load(object sender, EventArgs e)
		{
		}

		public void SetCBFunc(SetCfgCB setCfgCB, GetCfgCB getCfgCB, SendCfgDataCB sendCfgDataCB)
		{
			SetCfgCBFuncCB = setCfgCB;
			GetCfgCBFuncCB = getCfgCB;
			SendCfgDataCBFuncCB = sendCfgDataCB;
		}

		public void UpdateUiDisplay(UiParaInfoStu para)
		{
			ChbDMLowContrast.Checked = GetCfgCBFuncCB(53376u) == 128;
			ChbDM_ShortQZ.Checked = GetCfgCBFuncCB(21508u) == 4;
			if (EnUpdateBarcodeSwitch)
			{
				UpdateBarcodeSwitch();
			}
			if (para.DeviceTypeRecord >= 6)
			{
				GrbDPMSetting.Enabled = true;
				if (GetCfgCBFuncCB(198659u) == 1)
				{
					RdbDMShapeSquare.Checked = true;
				}
				else if (GetCfgCBFuncCB(198659u) == 2)
				{
					RdbDMShapeRectangle.Checked = true;
				}
				else
				{
					RdbDMShapeAll.Checked = true;
				}
				if (GetCfgCBFuncCB(198412u) == 8)
				{
					RdbDMGapMaxBig.Checked = true;
				}
				else if (GetCfgCBFuncCB(198412u) == 4)
				{
					RdbDMGapMaxSmall.Checked = true;
				}
				else
				{
					RdbDMGapMaxNone.Checked = true;
				}
				TxbModuleSizeMin.Text = GetCfgCBFuncCB(197887u).ToString();
				TxbModuleSizeMax.Text = GetCfgCBFuncCB(198143u).ToString();
				TxbDM_ModuleMin.Text = GetCfgCBFuncCB(196863u).ToString();
				TxbDM_ModuleMax.Text = GetCfgCBFuncCB(197119u).ToString();
				if (GetCfgCBFuncCB(198592u) == 64)
				{
					RdbDM_ColorSetWhite.Checked = true;
				}
				else if (GetCfgCBFuncCB(198592u) == 128)
				{
					RdbDM_ColorSetBlack.Checked = true;
				}
				else
				{
					RdbDM_ColorSetAny.Checked = true;
				}
				if (GetCfgCBFuncCB(198448u) == 16)
				{
					RdbDM_MirrorDisable.Checked = true;
				}
				else if (GetCfgCBFuncCB(198448u) == 32)
				{
					RdbDM_MirrorEnable.Checked = true;
				}
				else
				{
					RdbDM_MirrorAny.Checked = true;
				}
				ChbSmallCodeEnhance.Checked = GetCfgCBFuncCB(198672u) == 16;
				ChkDM_Not_ECC200.Checked = GetCfgCBFuncCB(198688u) == 32;
				ChkDMFindEnhance.Checked = GetCfgCBFuncCB(198664u) == 8;
				ChkDMDefilementEnhance.Checked = GetCfgCBFuncCB(198660u) == 4;
				ChbDM_SlantEnhance.Checked = GetCfgCBFuncCB(198399u) > 120;
			}
			else
			{
				GrbDPMSetting.Enabled = false;
			}
		}

		public void FunctionOnOff(bool[] CapacityArr)
		{
			GrbDPMSetting.Enabled = CapacityArr[14];
		}

		private void UpdateBarcodeSwitch()
		{
			if (GetCfgCBFuncCB(11777u) == 1)
			{
				ChbUPC_EAN.Checked = true;
			}
			else
			{
				ChbUPC_EAN.Checked = false;
			}
			if (GetCfgCBFuncCB(13057u) == 1)
			{
				ChbCode128.Checked = true;
			}
			else
			{
				ChbCode128.Checked = false;
			}
			if (GetCfgCBFuncCB(13825u) == 1)
			{
				ChbCode39.Checked = true;
			}
			else
			{
				ChbCode39.Checked = false;
			}
			if (GetCfgCBFuncCB(14593u) == 1)
			{
				ChbCode93.Checked = true;
			}
			else
			{
				ChbCode93.Checked = false;
			}
			if (GetCfgCBFuncCB(18689u) == 1)
			{
				ChbCode11.Checked = true;
			}
			else
			{
				ChbCode11.Checked = false;
			}
			if (GetCfgCBFuncCB(15361u) == 1)
			{
				ChbCodaBar.Checked = true;
			}
			else
			{
				ChbCodaBar.Checked = false;
			}
			if (GetCfgCBFuncCB(24321u) == 1)
			{
				ChbDataBar.Checked = true;
			}
			else
			{
				ChbDataBar.Checked = false;
			}
			if (GetCfgCBFuncCB(19457u) == 1)
			{
				ChbMSI.Checked = true;
			}
			else
			{
				ChbMSI.Checked = false;
			}
			if (GetCfgCBFuncCB(17153u) == 1)
			{
				ChbD25.Checked = true;
			}
			else
			{
				ChbD25.Checked = false;
			}
			if (GetCfgCBFuncCB(16385u) == 1)
			{
				ChbI25.Checked = true;
			}
			else
			{
				ChbI25.Checked = false;
			}
			if (GetCfgCBFuncCB(17921u) == 1)
			{
				ChbM25.Checked = true;
			}
			else
			{
				ChbM25.Checked = false;
			}
			if (GetCfgCBFuncCB(23297u) == 1)
			{
				ChbS25.Checked = true;
			}
			else
			{
				ChbS25.Checked = false;
			}
			if (GetCfgCBFuncCB(16129u) == 1)
			{
				ChbQRCode.Checked = true;
			}
			else
			{
				ChbQRCode.Checked = false;
			}
			if (GetCfgCBFuncCB(21761u) == 1)
			{
				ChbPDF417.Checked = true;
			}
			else
			{
				ChbPDF417.Checked = false;
			}
			if (GetCfgCBFuncCB(21505u) == 1)
			{
				ChbDM.Checked = true;
			}
			else
			{
				ChbDM.Checked = false;
			}
		}

		public void Page_Init()
		{
			BarcodeTypeCheckBox = new CheckBox[28]
			{
				ChbUPC_EAN, ChbUPC_EAN, ChbUPC_EAN, ChbUPC_EAN, ChbUPC_EAN, ChbCode128, ChbCode39, ChbCode93, ChbCodaBar, ChbI25,
				ChbD25, ChbM25, ChbCode11, ChbMSI, ChbDataBar, ChbDataBar, ChbDataBar, ChbQRCode, ChbDM, ChbPDF417,
				ChbAztec, ChbHanxin, ChbMicroPDF417, null, null, null, null, ChbMaxicode
			};
		}

		private void ChbUPC_EAN_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && EnUpdateBarcodeSwitch)
			{
				if (ChbUPC_EAN.Checked)
				{
					SetCfgCBFuncCB(11777u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(11777u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbCode128_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && EnUpdateBarcodeSwitch)
			{
				if (ChbCode128.Checked)
				{
					SetCfgCBFuncCB(13057u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(13057u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbCode39_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && EnUpdateBarcodeSwitch)
			{
				if (ChbCode39.Checked)
				{
					SetCfgCBFuncCB(13825u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(13825u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbCode93_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && EnUpdateBarcodeSwitch)
			{
				if (ChbCode93.Checked)
				{
					SetCfgCBFuncCB(14593u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(14593u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbDataBar_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && EnUpdateBarcodeSwitch)
			{
				if (ChbDataBar.Checked)
				{
					SetCfgCBFuncCB(24321u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(24321u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbCodaBar_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && EnUpdateBarcodeSwitch)
			{
				if (ChbCodaBar.Checked)
				{
					SetCfgCBFuncCB(15361u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(15361u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbD25_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && EnUpdateBarcodeSwitch)
			{
				if (ChbD25.Checked)
				{
					SetCfgCBFuncCB(17153u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(17153u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbI25_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && EnUpdateBarcodeSwitch)
			{
				if (ChbI25.Checked)
				{
					SetCfgCBFuncCB(16385u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(16385u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbS25_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && EnUpdateBarcodeSwitch)
			{
				if (ChbS25.Checked)
				{
					SetCfgCBFuncCB(23297u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(23297u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbCode11_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && EnUpdateBarcodeSwitch)
			{
				if (ChbCode11.Checked)
				{
					SetCfgCBFuncCB(18689u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(18689u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbMSI_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && EnUpdateBarcodeSwitch)
			{
				if (ChbMSI.Checked)
				{
					SetCfgCBFuncCB(19457u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(19457u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbM25_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && EnUpdateBarcodeSwitch)
			{
				if (ChbM25.Checked)
				{
					SetCfgCBFuncCB(17921u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(17921u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbQRCode_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && EnUpdateBarcodeSwitch)
			{
				if (ChbQRCode.Checked)
				{
					SetCfgCBFuncCB(16129u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(16129u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbDM_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && EnUpdateBarcodeSwitch)
			{
				if (ChbDM.Checked)
				{
					SetCfgCBFuncCB(21505u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(21505u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbPDF417_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && EnUpdateBarcodeSwitch)
			{
				if (ChbPDF417.Checked)
				{
					SetCfgCBFuncCB(21761u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(21761u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbAztec_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && EnUpdateBarcodeSwitch)
			{
				if (ChbAztec.Checked)
				{
					SetCfgCBFuncCB(22017u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(22017u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbHanxin_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && EnUpdateBarcodeSwitch)
			{
				if (ChbHanxin.Checked)
				{
					SetCfgCBFuncCB(22273u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(22273u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbMicroPDF417_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && EnUpdateBarcodeSwitch)
			{
				if (ChbMicroPDF417.Checked)
				{
					SetCfgCBFuncCB(22529u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(22529u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbGridMatrix_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void ChbDotCode_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void ChbMaxicode_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbMaxicode.Checked)
				{
					SetCfgCBFuncCB(23809u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(23809u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbDMShapeSquare_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbDMShapeSquare.Checked)
			{
				SetCfgCBFuncCB(198659u, 1u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbDMShapeRectangle_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbDMShapeRectangle.Checked)
			{
				SetCfgCBFuncCB(198659u, 2u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbDMShapeAll_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbDMShapeAll.Checked)
			{
				SetCfgCBFuncCB(198659u, 0u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbDMGapMaxNone_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbDMGapMaxNone.Checked)
			{
				SetCfgCBFuncCB(198412u, 0u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbDMGapMaxSmall_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbDMGapMaxSmall.Checked)
			{
				SetCfgCBFuncCB(198412u, 4u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbDMGapMaxBig_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbDMGapMaxBig.Checked)
			{
				SetCfgCBFuncCB(198412u, 8u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbDM_ColorSetWhite_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbDM_ColorSetWhite.Checked)
			{
				SetCfgCBFuncCB(198592u, 64u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbDM_ColorSetBlack_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbDM_ColorSetBlack.Checked)
			{
				SetCfgCBFuncCB(198592u, 128u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbDM_ColorSetAny_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbDM_ColorSetAny.Checked)
			{
				SetCfgCBFuncCB(198592u, 0u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbDM_MirrorEnable_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbDM_MirrorEnable.Checked)
			{
				SetCfgCBFuncCB(198448u, 32u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbDM_MirrorDisable_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbDM_MirrorDisable.Checked)
			{
				SetCfgCBFuncCB(198448u, 16u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbDM_MirrorAny_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbDM_MirrorAny.Checked)
			{
				SetCfgCBFuncCB(198448u, 0u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbDM_ShortQZ_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbDM_ShortQZ.Checked)
				{
					SetCfgCBFuncCB(21508u, 4u);
				}
				else
				{
					SetCfgCBFuncCB(21508u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbDMLowContrast_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbDMLowContrast.Checked)
				{
					SetCfgCBFuncCB(53376u, 128u);
				}
				else
				{
					SetCfgCBFuncCB(53376u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChkDM_Not_ECC200_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChkDM_Not_ECC200.Checked)
				{
					SetCfgCBFuncCB(198688u, 32u);
				}
				else
				{
					SetCfgCBFuncCB(198688u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChkDMFindEnhance_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChkDMFindEnhance.Checked)
				{
					SetCfgCBFuncCB(198664u, 8u);
				}
				else
				{
					SetCfgCBFuncCB(198664u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChkDMDefilementEnhance_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChkDMDefilementEnhance.Checked)
				{
					SetCfgCBFuncCB(198660u, 4u);
				}
				else
				{
					SetCfgCBFuncCB(198660u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbDM_SlantEnhance_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbDM_SlantEnhance.Checked)
				{
					SetCfgCBFuncCB(198399u, 255u);
				}
				else
				{
					SetCfgCBFuncCB(198399u, 84u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbSmallCodeEnhance_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbSmallCodeEnhance.Checked)
				{
					SetCfgCBFuncCB(198672u, 16u);
				}
				else
				{
					SetCfgCBFuncCB(198672u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void TxbModuleSizeMin_TextChanged(object sender, EventArgs e)
		{
			BtnModuleSizeMin.Visible = true;
		}

		private void BtnModuleSizeMin_Click(object sender, EventArgs e)
		{
			if (ToolCfg.UpdateAdjState)
			{
				return;
			}
			byte result = 0;
			if (byte.TryParse(TxbModuleSizeMin.Text, out result) && result < 100)
			{
				if (result > GetCfgCBFuncCB(198143u))
				{
					if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
					{
						MessageBox.Show("请确保输入数值比已经设置的最大值要小", "错误");
					}
					else
					{
						MessageBox.Show("Please enter a number greater than the right max value", "Error");
					}
				}
				else
				{
					SetCfgCBFuncCB(197887u, result);
					SendCfgDataCBFuncCB(0u);
					BtnModuleSizeMin.Visible = false;
				}
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				MessageBox.Show("请输入小于100的数", "错误");
			}
			else
			{
				MessageBox.Show("Please enter a number less than 100", "Error");
			}
		}

		private void TxbModuleSizeMax_TextChanged(object sender, EventArgs e)
		{
			BtnModuleSizeMax.Visible = true;
		}

		private void BtnModuleSizeMax_Click(object sender, EventArgs e)
		{
			if (ToolCfg.UpdateAdjState)
			{
				return;
			}
			byte result = 0;
			if (byte.TryParse(TxbModuleSizeMax.Text, out result) && result < 100)
			{
				if (result < GetCfgCBFuncCB(197887u))
				{
					if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
					{
						MessageBox.Show("请确保输入数值比已经设置的最小值要大", "错误");
					}
					else
					{
						MessageBox.Show("Please enter a number little than the left min value", "Error");
					}
				}
				else
				{
					SetCfgCBFuncCB(198143u, result);
					SendCfgDataCBFuncCB(0u);
					BtnModuleSizeMax.Visible = false;
				}
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				MessageBox.Show("请输入小于100的数", "错误");
			}
			else
			{
				MessageBox.Show("Please enter a number less than 100", "Error");
			}
		}

		private void TxbDM_ModuleMin_TextChanged(object sender, EventArgs e)
		{
			BtnDM_ModuleMin.Visible = true;
		}

		private void BtnDM_ModuleMin_Click(object sender, EventArgs e)
		{
			if (ToolCfg.UpdateAdjState)
			{
				return;
			}
			ushort result = 0;
			if (ushort.TryParse(TxbDM_ModuleMin.Text, out result) && result > 9)
			{
				if (result > GetCfgCBFuncCB(197119u))
				{
					if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
					{
						MessageBox.Show("请确保输入数值比已经设置的最大值要小", "错误");
					}
					else
					{
						MessageBox.Show("Please enter a number little than the right max value", "Error");
					}
				}
				else
				{
					SetCfgCBFuncCB(196863u, result & 0xFEu);
					SetCfgCBFuncCB(197375u, result & 0xFEu);
					SendCfgDataCBFuncCB(0u);
					BtnDM_ModuleMin.Visible = false;
				}
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				MessageBox.Show("请输入大于9的数", "错误");
			}
			else
			{
				MessageBox.Show("Please enter a number greater than 8", "Error");
			}
		}

		private void TxbDM_ModuleMax_TextChanged(object sender, EventArgs e)
		{
			BtnDM_ModuleMax.Visible = true;
		}

		private void BtnDM_ModuleMax_Click(object sender, EventArgs e)
		{
			if (ToolCfg.UpdateAdjState)
			{
				return;
			}
			ushort result = 0;
			if (ushort.TryParse(TxbDM_ModuleMax.Text, out result) && result <= 144)
			{
				if (result < GetCfgCBFuncCB(196863u))
				{
					if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
					{
						MessageBox.Show("请确保输入数值比已经设置的最小值要大", "错误");
					}
					else
					{
						MessageBox.Show("Please enter a number greater than the left min value", "Error");
					}
				}
				else
				{
					SetCfgCBFuncCB(197119u, result & 0xFEu);
					SetCfgCBFuncCB(197631u, result & 0xFEu);
					SendCfgDataCBFuncCB(0u);
					BtnDM_ModuleMax.Visible = false;
				}
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				MessageBox.Show("请输入小于等于144的数", "错误");
			}
			else
			{
				MessageBox.Show("Please enter a number little than 145", "Error");
			}
		}

		private void CheckBoxDefSet()
		{
			ChbUPC_EAN.Checked = true;
			ChbCode128.Checked = true;
			ChbCode39.Checked = true;
			ChbCode93.Checked = true;
			ChbCode11.Checked = false;
			ChbCodaBar.Checked = false;
			ChbDataBar.Checked = false;
			ChbMSI.Checked = false;
			ChbD25.Checked = false;
			ChbI25.Checked = false;
			ChbM25.Checked = false;
			ChbS25.Checked = false;
			ChbQRCode.Checked = true;
			ChbPDF417.Checked = false;
			ChbDM.Checked = true;
		}

		private void CheckBoxAllSet(bool is_sel)
		{
			ChbUPC_EAN.Checked = is_sel;
			ChbCode128.Checked = is_sel;
			ChbCode39.Checked = is_sel;
			ChbCode93.Checked = is_sel;
			ChbCode11.Checked = is_sel;
			ChbCodaBar.Checked = is_sel;
			ChbDataBar.Checked = is_sel;
			ChbMSI.Checked = is_sel;
			ChbD25.Checked = is_sel;
			ChbI25.Checked = is_sel;
			ChbM25.Checked = is_sel;
			ChbS25.Checked = is_sel;
			ChbQRCode.Checked = is_sel;
			ChbPDF417.Checked = is_sel;
			ChbDM.Checked = is_sel;
		}

		public void BtnOpenDefaultBarcodeType_Click(object sender, EventArgs e)
		{
			EnUpdateBarcodeSwitch = false;
			CheckBoxDefSet();
			EnUpdateBarcodeSwitch = true;
			SendCfgDataCBFuncCB(4096u);
		}

		private void BtnOpenAllBarcodeType_Click(object sender, EventArgs e)
		{
			EnUpdateBarcodeSwitch = false;
			CheckBoxAllSet(true);
			EnUpdateBarcodeSwitch = true;
			SendCfgDataCBFuncCB(8192u);
		}

		private void BtnCloseAllBarcodeType_Click(object sender, EventArgs e)
		{
			EnUpdateBarcodeSwitch = false;
			CheckBoxAllSet(false);
			EnUpdateBarcodeSwitch = true;
			SendCfgDataCBFuncCB(2048u);
		}

		public void ShowBarcodeType()
		{
			EnUpdateBarcodeSwitch = false;
			UpdateBarcodeSwitch();
			EnUpdateBarcodeSwitch = true;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.GrbOtherBarcode = new System.Windows.Forms.GroupBox();
			this.ChbGridMatrix = new System.Windows.Forms.CheckBox();
			this.ChbDotCode = new System.Windows.Forms.CheckBox();
			this.ChbMaxicode = new System.Windows.Forms.CheckBox();
			this.ChbMicroPDF417 = new System.Windows.Forms.CheckBox();
			this.ChbHanxin = new System.Windows.Forms.CheckBox();
			this.ChbAztec = new System.Windows.Forms.CheckBox();
			this.GrbUsually2D = new System.Windows.Forms.GroupBox();
			this.ChbPDF417 = new System.Windows.Forms.CheckBox();
			this.ChbDM = new System.Windows.Forms.CheckBox();
			this.ChbQRCode = new System.Windows.Forms.CheckBox();
			this.GrbUsually1D = new System.Windows.Forms.GroupBox();
			this.ChbCode93 = new System.Windows.Forms.CheckBox();
			this.ChbDataBar = new System.Windows.Forms.CheckBox();
			this.ChbS25 = new System.Windows.Forms.CheckBox();
			this.ChbM25 = new System.Windows.Forms.CheckBox();
			this.ChbD25 = new System.Windows.Forms.CheckBox();
			this.ChbCode11 = new System.Windows.Forms.CheckBox();
			this.ChbI25 = new System.Windows.Forms.CheckBox();
			this.ChbCodaBar = new System.Windows.Forms.CheckBox();
			this.ChbMSI = new System.Windows.Forms.CheckBox();
			this.ChbCode39 = new System.Windows.Forms.CheckBox();
			this.ChbCode128 = new System.Windows.Forms.CheckBox();
			this.ChbUPC_EAN = new System.Windows.Forms.CheckBox();
			this.BtnOpenAllBarcodeType = new System.Windows.Forms.Button();
			this.BtnOpenDefaultBarcodeType = new System.Windows.Forms.Button();
			this.BtnCloseAllBarcodeType = new System.Windows.Forms.Button();
			this.GrbDPMSetting = new System.Windows.Forms.GroupBox();
			this.ChbDM_ShortQZ = new System.Windows.Forms.CheckBox();
			this.ChkDM_Not_ECC200 = new System.Windows.Forms.CheckBox();
			this.PanDmMirror = new System.Windows.Forms.Panel();
			this.RdbDM_MirrorAny = new System.Windows.Forms.RadioButton();
			this.RdbDM_MirrorDisable = new System.Windows.Forms.RadioButton();
			this.RdbDM_MirrorEnable = new System.Windows.Forms.RadioButton();
			this.LabDM_MirrorSet = new System.Windows.Forms.Label();
			this.PanDmColor = new System.Windows.Forms.Panel();
			this.LabDM_ColorSet = new System.Windows.Forms.Label();
			this.RdbDM_ColorSetAny = new System.Windows.Forms.RadioButton();
			this.RdbDM_ColorSetBlack = new System.Windows.Forms.RadioButton();
			this.RdbDM_ColorSetWhite = new System.Windows.Forms.RadioButton();
			this.ChbSmallCodeEnhance = new System.Windows.Forms.CheckBox();
			this.ChbDM_SlantEnhance = new System.Windows.Forms.CheckBox();
			this.PanDmModuleMin = new System.Windows.Forms.Panel();
			this.TxbDM_ModuleMax = new System.Windows.Forms.TextBox();
			this.TxbDM_ModuleMin = new System.Windows.Forms.TextBox();
			this.BtnDM_ModuleMax = new System.Windows.Forms.Button();
			this.LabDM_ModuleMax = new System.Windows.Forms.Label();
			this.BtnDM_ModuleMin = new System.Windows.Forms.Button();
			this.LabDM_ModuleMin = new System.Windows.Forms.Label();
			this.PanDmModuleSizeMin = new System.Windows.Forms.Panel();
			this.TxbModuleSizeMax = new System.Windows.Forms.TextBox();
			this.TxbModuleSizeMin = new System.Windows.Forms.TextBox();
			this.BtnModuleSizeMax = new System.Windows.Forms.Button();
			this.LabDMModuleSizeMax = new System.Windows.Forms.Label();
			this.BtnModuleSizeMin = new System.Windows.Forms.Button();
			this.LabDMModuleSizeMin = new System.Windows.Forms.Label();
			this.PanDmMaxGap = new System.Windows.Forms.Panel();
			this.LabDMGapMax = new System.Windows.Forms.Label();
			this.RdbDMGapMaxBig = new System.Windows.Forms.RadioButton();
			this.RdbDMGapMaxSmall = new System.Windows.Forms.RadioButton();
			this.RdbDMGapMaxNone = new System.Windows.Forms.RadioButton();
			this.ChkDMDefilementEnhance = new System.Windows.Forms.CheckBox();
			this.ChkDMFindEnhance = new System.Windows.Forms.CheckBox();
			this.PanDm2dType = new System.Windows.Forms.Panel();
			this.LabDm2dType = new System.Windows.Forms.Label();
			this.RdbDMShapeAll = new System.Windows.Forms.RadioButton();
			this.RdbDMShapeRectangle = new System.Windows.Forms.RadioButton();
			this.RdbDMShapeSquare = new System.Windows.Forms.RadioButton();
			this.ChbDMLowContrast = new System.Windows.Forms.CheckBox();
			this.GrbOtherBarcode.SuspendLayout();
			this.GrbUsually2D.SuspendLayout();
			this.GrbUsually1D.SuspendLayout();
			this.GrbDPMSetting.SuspendLayout();
			this.PanDmMirror.SuspendLayout();
			this.PanDmColor.SuspendLayout();
			this.PanDmModuleMin.SuspendLayout();
			this.PanDmModuleSizeMin.SuspendLayout();
			this.PanDmMaxGap.SuspendLayout();
			this.PanDm2dType.SuspendLayout();
			base.SuspendLayout();
			this.GrbOtherBarcode.Controls.Add(this.ChbGridMatrix);
			this.GrbOtherBarcode.Controls.Add(this.ChbDotCode);
			this.GrbOtherBarcode.Controls.Add(this.ChbMaxicode);
			this.GrbOtherBarcode.Controls.Add(this.ChbMicroPDF417);
			this.GrbOtherBarcode.Controls.Add(this.ChbHanxin);
			this.GrbOtherBarcode.Controls.Add(this.ChbAztec);
			this.GrbOtherBarcode.Enabled = false;
			this.GrbOtherBarcode.Location = new System.Drawing.Point(3, 176);
			this.GrbOtherBarcode.Name = "GrbOtherBarcode";
			this.GrbOtherBarcode.Size = new System.Drawing.Size(402, 81);
			this.GrbOtherBarcode.TabIndex = 5;
			this.GrbOtherBarcode.TabStop = false;
			this.GrbOtherBarcode.Text = "其他条码";
			this.ChbGridMatrix.AutoSize = true;
			this.ChbGridMatrix.Location = new System.Drawing.Point(36, 53);
			this.ChbGridMatrix.Name = "ChbGridMatrix";
			this.ChbGridMatrix.Size = new System.Drawing.Size(96, 18);
			this.ChbGridMatrix.TabIndex = 39;
			this.ChbGridMatrix.Text = "GridMatrix";
			this.ChbGridMatrix.UseVisualStyleBackColor = true;
			this.ChbGridMatrix.CheckedChanged += new System.EventHandler(ChbGridMatrix_CheckedChanged);
			this.ChbDotCode.AutoSize = true;
			this.ChbDotCode.Location = new System.Drawing.Point(157, 53);
			this.ChbDotCode.Name = "ChbDotCode";
			this.ChbDotCode.Size = new System.Drawing.Size(75, 18);
			this.ChbDotCode.TabIndex = 38;
			this.ChbDotCode.Text = "DotCode";
			this.ChbDotCode.UseVisualStyleBackColor = true;
			this.ChbDotCode.CheckedChanged += new System.EventHandler(ChbDotCode_CheckedChanged);
			this.ChbMaxicode.AutoSize = true;
			this.ChbMaxicode.Location = new System.Drawing.Point(257, 53);
			this.ChbMaxicode.Name = "ChbMaxicode";
			this.ChbMaxicode.Size = new System.Drawing.Size(82, 18);
			this.ChbMaxicode.TabIndex = 37;
			this.ChbMaxicode.Text = "Maxicode";
			this.ChbMaxicode.UseVisualStyleBackColor = true;
			this.ChbMaxicode.CheckedChanged += new System.EventHandler(ChbMaxicode_CheckedChanged);
			this.ChbMicroPDF417.AutoSize = true;
			this.ChbMicroPDF417.Location = new System.Drawing.Point(257, 29);
			this.ChbMicroPDF417.Name = "ChbMicroPDF417";
			this.ChbMicroPDF417.Size = new System.Drawing.Size(103, 18);
			this.ChbMicroPDF417.TabIndex = 36;
			this.ChbMicroPDF417.Text = "MicroPDF417";
			this.ChbMicroPDF417.UseVisualStyleBackColor = true;
			this.ChbMicroPDF417.CheckedChanged += new System.EventHandler(ChbMicroPDF417_CheckedChanged);
			this.ChbHanxin.AutoSize = true;
			this.ChbHanxin.Location = new System.Drawing.Point(157, 29);
			this.ChbHanxin.Name = "ChbHanxin";
			this.ChbHanxin.Size = new System.Drawing.Size(68, 18);
			this.ChbHanxin.TabIndex = 35;
			this.ChbHanxin.Text = "Hanxin";
			this.ChbHanxin.UseVisualStyleBackColor = true;
			this.ChbHanxin.CheckedChanged += new System.EventHandler(ChbHanxin_CheckedChanged);
			this.ChbAztec.AutoSize = true;
			this.ChbAztec.Location = new System.Drawing.Point(36, 29);
			this.ChbAztec.Name = "ChbAztec";
			this.ChbAztec.Size = new System.Drawing.Size(61, 18);
			this.ChbAztec.TabIndex = 34;
			this.ChbAztec.Text = "Aztec";
			this.ChbAztec.UseVisualStyleBackColor = true;
			this.ChbAztec.CheckedChanged += new System.EventHandler(ChbAztec_CheckedChanged);
			this.GrbUsually2D.Controls.Add(this.ChbPDF417);
			this.GrbUsually2D.Controls.Add(this.ChbDM);
			this.GrbUsually2D.Controls.Add(this.ChbQRCode);
			this.GrbUsually2D.Location = new System.Drawing.Point(3, 111);
			this.GrbUsually2D.Name = "GrbUsually2D";
			this.GrbUsually2D.Size = new System.Drawing.Size(402, 59);
			this.GrbUsually2D.TabIndex = 4;
			this.GrbUsually2D.TabStop = false;
			this.GrbUsually2D.Text = "常用二维码";
			this.ChbPDF417.AutoSize = true;
			this.ChbPDF417.Checked = true;
			this.ChbPDF417.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChbPDF417.Location = new System.Drawing.Point(260, 29);
			this.ChbPDF417.Name = "ChbPDF417";
			this.ChbPDF417.Size = new System.Drawing.Size(68, 18);
			this.ChbPDF417.TabIndex = 33;
			this.ChbPDF417.Text = "PDF417";
			this.ChbPDF417.UseVisualStyleBackColor = true;
			this.ChbPDF417.CheckedChanged += new System.EventHandler(ChbPDF417_CheckedChanged);
			this.ChbDM.AutoSize = true;
			this.ChbDM.Checked = true;
			this.ChbDM.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChbDM.Location = new System.Drawing.Point(136, 29);
			this.ChbDM.Name = "ChbDM";
			this.ChbDM.Size = new System.Drawing.Size(96, 18);
			this.ChbDM.TabIndex = 32;
			this.ChbDM.Text = "DataMatrix";
			this.ChbDM.UseVisualStyleBackColor = true;
			this.ChbDM.CheckedChanged += new System.EventHandler(ChbDM_CheckedChanged);
			this.ChbQRCode.AutoSize = true;
			this.ChbQRCode.Checked = true;
			this.ChbQRCode.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChbQRCode.Location = new System.Drawing.Point(36, 29);
			this.ChbQRCode.Name = "ChbQRCode";
			this.ChbQRCode.Size = new System.Drawing.Size(68, 18);
			this.ChbQRCode.TabIndex = 31;
			this.ChbQRCode.Text = "QRCode";
			this.ChbQRCode.UseVisualStyleBackColor = true;
			this.ChbQRCode.CheckedChanged += new System.EventHandler(ChbQRCode_CheckedChanged);
			this.GrbUsually1D.Controls.Add(this.ChbCode93);
			this.GrbUsually1D.Controls.Add(this.ChbDataBar);
			this.GrbUsually1D.Controls.Add(this.ChbS25);
			this.GrbUsually1D.Controls.Add(this.ChbM25);
			this.GrbUsually1D.Controls.Add(this.ChbD25);
			this.GrbUsually1D.Controls.Add(this.ChbCode11);
			this.GrbUsually1D.Controls.Add(this.ChbI25);
			this.GrbUsually1D.Controls.Add(this.ChbCodaBar);
			this.GrbUsually1D.Controls.Add(this.ChbMSI);
			this.GrbUsually1D.Controls.Add(this.ChbCode39);
			this.GrbUsually1D.Controls.Add(this.ChbCode128);
			this.GrbUsually1D.Controls.Add(this.ChbUPC_EAN);
			this.GrbUsually1D.Location = new System.Drawing.Point(3, 3);
			this.GrbUsually1D.Name = "GrbUsually1D";
			this.GrbUsually1D.Size = new System.Drawing.Size(402, 102);
			this.GrbUsually1D.TabIndex = 3;
			this.GrbUsually1D.TabStop = false;
			this.GrbUsually1D.Text = "常用一维码";
			this.ChbCode93.AutoSize = true;
			this.ChbCode93.Checked = true;
			this.ChbCode93.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChbCode93.Location = new System.Drawing.Point(329, 29);
			this.ChbCode93.Name = "ChbCode93";
			this.ChbCode93.Size = new System.Drawing.Size(68, 18);
			this.ChbCode93.TabIndex = 24;
			this.ChbCode93.Text = "Code93";
			this.ChbCode93.UseVisualStyleBackColor = true;
			this.ChbCode93.CheckedChanged += new System.EventHandler(ChbCode93_CheckedChanged);
			this.ChbDataBar.AutoSize = true;
			this.ChbDataBar.Checked = true;
			this.ChbDataBar.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChbDataBar.Location = new System.Drawing.Point(36, 53);
			this.ChbDataBar.Name = "ChbDataBar";
			this.ChbDataBar.Size = new System.Drawing.Size(75, 18);
			this.ChbDataBar.TabIndex = 30;
			this.ChbDataBar.Text = "DataBar";
			this.ChbDataBar.UseVisualStyleBackColor = true;
			this.ChbDataBar.CheckedChanged += new System.EventHandler(ChbDataBar_CheckedChanged);
			this.ChbS25.AutoSize = true;
			this.ChbS25.Checked = true;
			this.ChbS25.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChbS25.Location = new System.Drawing.Point(36, 77);
			this.ChbS25.Name = "ChbS25";
			this.ChbS25.Size = new System.Drawing.Size(47, 18);
			this.ChbS25.TabIndex = 29;
			this.ChbS25.Text = "S25";
			this.ChbS25.UseVisualStyleBackColor = true;
			this.ChbS25.CheckedChanged += new System.EventHandler(ChbS25_CheckedChanged);
			this.ChbM25.AutoSize = true;
			this.ChbM25.Checked = true;
			this.ChbM25.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChbM25.Location = new System.Drawing.Point(329, 77);
			this.ChbM25.Name = "ChbM25";
			this.ChbM25.Size = new System.Drawing.Size(47, 18);
			this.ChbM25.TabIndex = 28;
			this.ChbM25.Text = "M25";
			this.ChbM25.UseVisualStyleBackColor = true;
			this.ChbM25.CheckedChanged += new System.EventHandler(ChbM25_CheckedChanged);
			this.ChbD25.AutoSize = true;
			this.ChbD25.Checked = true;
			this.ChbD25.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChbD25.Location = new System.Drawing.Point(236, 53);
			this.ChbD25.Name = "ChbD25";
			this.ChbD25.Size = new System.Drawing.Size(47, 18);
			this.ChbD25.TabIndex = 27;
			this.ChbD25.Text = "D25";
			this.ChbD25.UseVisualStyleBackColor = true;
			this.ChbD25.CheckedChanged += new System.EventHandler(ChbD25_CheckedChanged);
			this.ChbCode11.AutoSize = true;
			this.ChbCode11.Checked = true;
			this.ChbCode11.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChbCode11.Location = new System.Drawing.Point(136, 77);
			this.ChbCode11.Name = "ChbCode11";
			this.ChbCode11.Size = new System.Drawing.Size(68, 18);
			this.ChbCode11.TabIndex = 26;
			this.ChbCode11.Text = "Code11";
			this.ChbCode11.UseVisualStyleBackColor = true;
			this.ChbCode11.CheckedChanged += new System.EventHandler(ChbCode11_CheckedChanged);
			this.ChbI25.AutoSize = true;
			this.ChbI25.Checked = true;
			this.ChbI25.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChbI25.Location = new System.Drawing.Point(329, 53);
			this.ChbI25.Name = "ChbI25";
			this.ChbI25.Size = new System.Drawing.Size(47, 18);
			this.ChbI25.TabIndex = 25;
			this.ChbI25.Text = "I25";
			this.ChbI25.UseVisualStyleBackColor = true;
			this.ChbI25.CheckedChanged += new System.EventHandler(ChbI25_CheckedChanged);
			this.ChbCodaBar.AutoSize = true;
			this.ChbCodaBar.Checked = true;
			this.ChbCodaBar.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChbCodaBar.Location = new System.Drawing.Point(136, 53);
			this.ChbCodaBar.Name = "ChbCodaBar";
			this.ChbCodaBar.Size = new System.Drawing.Size(75, 18);
			this.ChbCodaBar.TabIndex = 23;
			this.ChbCodaBar.Text = "CodaBar";
			this.ChbCodaBar.UseVisualStyleBackColor = true;
			this.ChbCodaBar.CheckedChanged += new System.EventHandler(ChbCodaBar_CheckedChanged);
			this.ChbMSI.AutoSize = true;
			this.ChbMSI.Checked = true;
			this.ChbMSI.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChbMSI.Location = new System.Drawing.Point(236, 77);
			this.ChbMSI.Name = "ChbMSI";
			this.ChbMSI.Size = new System.Drawing.Size(47, 18);
			this.ChbMSI.TabIndex = 22;
			this.ChbMSI.Text = "MSI";
			this.ChbMSI.UseVisualStyleBackColor = true;
			this.ChbMSI.CheckedChanged += new System.EventHandler(ChbMSI_CheckedChanged);
			this.ChbCode39.AutoSize = true;
			this.ChbCode39.Checked = true;
			this.ChbCode39.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChbCode39.Location = new System.Drawing.Point(236, 29);
			this.ChbCode39.Name = "ChbCode39";
			this.ChbCode39.Size = new System.Drawing.Size(68, 18);
			this.ChbCode39.TabIndex = 21;
			this.ChbCode39.Text = "Code39";
			this.ChbCode39.UseVisualStyleBackColor = true;
			this.ChbCode39.CheckedChanged += new System.EventHandler(ChbCode39_CheckedChanged);
			this.ChbCode128.AutoSize = true;
			this.ChbCode128.Checked = true;
			this.ChbCode128.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChbCode128.Location = new System.Drawing.Point(136, 29);
			this.ChbCode128.Name = "ChbCode128";
			this.ChbCode128.Size = new System.Drawing.Size(75, 18);
			this.ChbCode128.TabIndex = 20;
			this.ChbCode128.Text = "Code128";
			this.ChbCode128.UseVisualStyleBackColor = true;
			this.ChbCode128.CheckedChanged += new System.EventHandler(ChbCode128_CheckedChanged);
			this.ChbUPC_EAN.AutoSize = true;
			this.ChbUPC_EAN.Checked = true;
			this.ChbUPC_EAN.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChbUPC_EAN.Location = new System.Drawing.Point(36, 29);
			this.ChbUPC_EAN.Name = "ChbUPC_EAN";
			this.ChbUPC_EAN.Size = new System.Drawing.Size(75, 18);
			this.ChbUPC_EAN.TabIndex = 19;
			this.ChbUPC_EAN.Text = "UPC/EAN";
			this.ChbUPC_EAN.UseVisualStyleBackColor = true;
			this.ChbUPC_EAN.CheckedChanged += new System.EventHandler(ChbUPC_EAN_CheckedChanged);
			this.BtnOpenAllBarcodeType.Location = new System.Drawing.Point(169, 263);
			this.BtnOpenAllBarcodeType.Name = "BtnOpenAllBarcodeType";
			this.BtnOpenAllBarcodeType.Size = new System.Drawing.Size(128, 35);
			this.BtnOpenAllBarcodeType.TabIndex = 9;
			this.BtnOpenAllBarcodeType.Text = "开启所有常用码制";
			this.BtnOpenAllBarcodeType.UseVisualStyleBackColor = true;
			this.BtnOpenAllBarcodeType.Click += new System.EventHandler(BtnOpenAllBarcodeType_Click);
			this.BtnOpenDefaultBarcodeType.Location = new System.Drawing.Point(30, 263);
			this.BtnOpenDefaultBarcodeType.Name = "BtnOpenDefaultBarcodeType";
			this.BtnOpenDefaultBarcodeType.Size = new System.Drawing.Size(128, 35);
			this.BtnOpenDefaultBarcodeType.TabIndex = 8;
			this.BtnOpenDefaultBarcodeType.Text = "开启默认常用码制";
			this.BtnOpenDefaultBarcodeType.UseVisualStyleBackColor = true;
			this.BtnOpenDefaultBarcodeType.Click += new System.EventHandler(BtnOpenDefaultBarcodeType_Click);
			this.BtnCloseAllBarcodeType.Location = new System.Drawing.Point(30, 304);
			this.BtnCloseAllBarcodeType.Name = "BtnCloseAllBarcodeType";
			this.BtnCloseAllBarcodeType.Size = new System.Drawing.Size(128, 35);
			this.BtnCloseAllBarcodeType.TabIndex = 7;
			this.BtnCloseAllBarcodeType.Text = "关闭所有码制";
			this.BtnCloseAllBarcodeType.UseVisualStyleBackColor = true;
			this.BtnCloseAllBarcodeType.Click += new System.EventHandler(BtnCloseAllBarcodeType_Click);
			this.GrbDPMSetting.Controls.Add(this.ChkDM_Not_ECC200);
			this.GrbDPMSetting.Controls.Add(this.PanDmMirror);
			this.GrbDPMSetting.Controls.Add(this.PanDmColor);
			this.GrbDPMSetting.Controls.Add(this.ChbSmallCodeEnhance);
			this.GrbDPMSetting.Controls.Add(this.ChbDM_SlantEnhance);
			this.GrbDPMSetting.Controls.Add(this.PanDmModuleMin);
			this.GrbDPMSetting.Controls.Add(this.PanDmModuleSizeMin);
			this.GrbDPMSetting.Controls.Add(this.PanDmMaxGap);
			this.GrbDPMSetting.Controls.Add(this.ChkDMDefilementEnhance);
			this.GrbDPMSetting.Controls.Add(this.ChkDMFindEnhance);
			this.GrbDPMSetting.Controls.Add(this.PanDm2dType);
			this.GrbDPMSetting.Controls.Add(this.ChbDMLowContrast);
			this.GrbDPMSetting.Controls.Add(this.ChbDM_ShortQZ);
			this.GrbDPMSetting.Location = new System.Drawing.Point(3, 355);
			this.GrbDPMSetting.Name = "GrbDPMSetting";
			this.GrbDPMSetting.Size = new System.Drawing.Size(402, 284);
			this.GrbDPMSetting.TabIndex = 10;
			this.GrbDPMSetting.TabStop = false;
			this.GrbDPMSetting.Text = "DPM相关设置";
			this.ChbDM_ShortQZ.AutoSize = true;
			this.ChbDM_ShortQZ.Location = new System.Drawing.Point(40, 159);
			this.ChbDM_ShortQZ.Name = "ChbDM_ShortQZ";
			this.ChbDM_ShortQZ.Size = new System.Drawing.Size(68, 18);
			this.ChbDM_ShortQZ.TabIndex = 52;
			this.ChbDM_ShortQZ.Text = "短静区";
			this.ChbDM_ShortQZ.UseVisualStyleBackColor = true;
			this.ChbDM_ShortQZ.CheckedChanged += new System.EventHandler(ChbDM_ShortQZ_CheckedChanged);
			this.ChkDM_Not_ECC200.AutoSize = true;
			this.ChkDM_Not_ECC200.Location = new System.Drawing.Point(266, 159);
			this.ChkDM_Not_ECC200.Name = "ChkDM_Not_ECC200";
			this.ChkDM_Not_ECC200.Size = new System.Drawing.Size(117, 18);
			this.ChkDM_Not_ECC200.TabIndex = 51;
			this.ChkDM_Not_ECC200.Text = "非ECC-200解码";
			this.ChkDM_Not_ECC200.UseVisualStyleBackColor = true;
			this.ChkDM_Not_ECC200.CheckedChanged += new System.EventHandler(ChkDM_Not_ECC200_CheckedChanged);
			this.PanDmMirror.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.PanDmMirror.Controls.Add(this.RdbDM_MirrorAny);
			this.PanDmMirror.Controls.Add(this.RdbDM_MirrorDisable);
			this.PanDmMirror.Controls.Add(this.RdbDM_MirrorEnable);
			this.PanDmMirror.Controls.Add(this.LabDM_MirrorSet);
			this.PanDmMirror.Location = new System.Drawing.Point(36, 121);
			this.PanDmMirror.Name = "PanDmMirror";
			this.PanDmMirror.Size = new System.Drawing.Size(353, 25);
			this.PanDmMirror.TabIndex = 46;
			this.RdbDM_MirrorAny.AutoSize = true;
			this.RdbDM_MirrorAny.Location = new System.Drawing.Point(227, 1);
			this.RdbDM_MirrorAny.Name = "RdbDM_MirrorAny";
			this.RdbDM_MirrorAny.Size = new System.Drawing.Size(53, 18);
			this.RdbDM_MirrorAny.TabIndex = 40;
			this.RdbDM_MirrorAny.Text = "任意";
			this.RdbDM_MirrorAny.UseVisualStyleBackColor = true;
			this.RdbDM_MirrorAny.CheckedChanged += new System.EventHandler(RdbDM_MirrorAny_CheckedChanged);
			this.RdbDM_MirrorDisable.AutoSize = true;
			this.RdbDM_MirrorDisable.Location = new System.Drawing.Point(150, 1);
			this.RdbDM_MirrorDisable.Name = "RdbDM_MirrorDisable";
			this.RdbDM_MirrorDisable.Size = new System.Drawing.Size(53, 18);
			this.RdbDM_MirrorDisable.TabIndex = 39;
			this.RdbDM_MirrorDisable.Text = "关闭";
			this.RdbDM_MirrorDisable.UseVisualStyleBackColor = true;
			this.RdbDM_MirrorDisable.CheckedChanged += new System.EventHandler(RdbDM_MirrorDisable_CheckedChanged);
			this.RdbDM_MirrorEnable.AutoSize = true;
			this.RdbDM_MirrorEnable.Location = new System.Drawing.Point(75, 1);
			this.RdbDM_MirrorEnable.Name = "RdbDM_MirrorEnable";
			this.RdbDM_MirrorEnable.Size = new System.Drawing.Size(53, 18);
			this.RdbDM_MirrorEnable.TabIndex = 38;
			this.RdbDM_MirrorEnable.Text = "镜像";
			this.RdbDM_MirrorEnable.UseVisualStyleBackColor = true;
			this.RdbDM_MirrorEnable.CheckedChanged += new System.EventHandler(RdbDM_MirrorEnable_CheckedChanged);
			this.LabDM_MirrorSet.AutoSize = true;
			this.LabDM_MirrorSet.Location = new System.Drawing.Point(1, 2);
			this.LabDM_MirrorSet.Name = "LabDM_MirrorSet";
			this.LabDM_MirrorSet.Size = new System.Drawing.Size(70, 14);
			this.LabDM_MirrorSet.TabIndex = 43;
			this.LabDM_MirrorSet.Text = "DM码镜像:";
			this.PanDmColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.PanDmColor.Controls.Add(this.LabDM_ColorSet);
			this.PanDmColor.Controls.Add(this.RdbDM_ColorSetAny);
			this.PanDmColor.Controls.Add(this.RdbDM_ColorSetBlack);
			this.PanDmColor.Controls.Add(this.RdbDM_ColorSetWhite);
			this.PanDmColor.Location = new System.Drawing.Point(36, 91);
			this.PanDmColor.Name = "PanDmColor";
			this.PanDmColor.Size = new System.Drawing.Size(353, 25);
			this.PanDmColor.TabIndex = 45;
			this.LabDM_ColorSet.AutoSize = true;
			this.LabDM_ColorSet.Location = new System.Drawing.Point(1, 2);
			this.LabDM_ColorSet.Name = "LabDM_ColorSet";
			this.LabDM_ColorSet.Size = new System.Drawing.Size(70, 14);
			this.LabDM_ColorSet.TabIndex = 43;
			this.LabDM_ColorSet.Text = "DM码颜色:";
			this.RdbDM_ColorSetAny.AutoSize = true;
			this.RdbDM_ColorSetAny.Location = new System.Drawing.Point(227, 1);
			this.RdbDM_ColorSetAny.Name = "RdbDM_ColorSetAny";
			this.RdbDM_ColorSetAny.Size = new System.Drawing.Size(53, 18);
			this.RdbDM_ColorSetAny.TabIndex = 40;
			this.RdbDM_ColorSetAny.Text = "任意";
			this.RdbDM_ColorSetAny.UseVisualStyleBackColor = true;
			this.RdbDM_ColorSetAny.CheckedChanged += new System.EventHandler(RdbDM_ColorSetAny_CheckedChanged);
			this.RdbDM_ColorSetBlack.AutoSize = true;
			this.RdbDM_ColorSetBlack.Location = new System.Drawing.Point(151, 1);
			this.RdbDM_ColorSetBlack.Name = "RdbDM_ColorSetBlack";
			this.RdbDM_ColorSetBlack.Size = new System.Drawing.Size(53, 18);
			this.RdbDM_ColorSetBlack.TabIndex = 39;
			this.RdbDM_ColorSetBlack.Text = "黑底";
			this.RdbDM_ColorSetBlack.UseVisualStyleBackColor = true;
			this.RdbDM_ColorSetBlack.CheckedChanged += new System.EventHandler(RdbDM_ColorSetBlack_CheckedChanged);
			this.RdbDM_ColorSetWhite.AutoSize = true;
			this.RdbDM_ColorSetWhite.Location = new System.Drawing.Point(75, 1);
			this.RdbDM_ColorSetWhite.Name = "RdbDM_ColorSetWhite";
			this.RdbDM_ColorSetWhite.Size = new System.Drawing.Size(53, 18);
			this.RdbDM_ColorSetWhite.TabIndex = 38;
			this.RdbDM_ColorSetWhite.Text = "白底";
			this.RdbDM_ColorSetWhite.UseVisualStyleBackColor = true;
			this.RdbDM_ColorSetWhite.CheckedChanged += new System.EventHandler(RdbDM_ColorSetWhite_CheckedChanged);
			this.ChbSmallCodeEnhance.AutoSize = true;
			this.ChbSmallCodeEnhance.Location = new System.Drawing.Point(301, 183);
			this.ChbSmallCodeEnhance.Name = "ChbSmallCodeEnhance";
			this.ChbSmallCodeEnhance.Size = new System.Drawing.Size(82, 18);
			this.ChbSmallCodeEnhance.TabIndex = 50;
			this.ChbSmallCodeEnhance.Text = "小码增强";
			this.ChbSmallCodeEnhance.UseVisualStyleBackColor = true;
			this.ChbSmallCodeEnhance.CheckedChanged += new System.EventHandler(ChbSmallCodeEnhance_CheckedChanged);
			this.ChbDM_SlantEnhance.AutoSize = true;
			this.ChbDM_SlantEnhance.Location = new System.Drawing.Point(214, 183);
			this.ChbDM_SlantEnhance.Name = "ChbDM_SlantEnhance";
			this.ChbDM_SlantEnhance.Size = new System.Drawing.Size(82, 18);
			this.ChbDM_SlantEnhance.TabIndex = 49;
			this.ChbDM_SlantEnhance.Text = "变形增强";
			this.ChbDM_SlantEnhance.UseVisualStyleBackColor = true;
			this.ChbDM_SlantEnhance.CheckedChanged += new System.EventHandler(ChbDM_SlantEnhance_CheckedChanged);
			this.PanDmModuleMin.Controls.Add(this.TxbDM_ModuleMax);
			this.PanDmModuleMin.Controls.Add(this.TxbDM_ModuleMin);
			this.PanDmModuleMin.Controls.Add(this.BtnDM_ModuleMax);
			this.PanDmModuleMin.Controls.Add(this.LabDM_ModuleMax);
			this.PanDmModuleMin.Controls.Add(this.BtnDM_ModuleMin);
			this.PanDmModuleMin.Controls.Add(this.LabDM_ModuleMin);
			this.PanDmModuleMin.Location = new System.Drawing.Point(36, 245);
			this.PanDmModuleMin.Name = "PanDmModuleMin";
			this.PanDmModuleMin.Size = new System.Drawing.Size(353, 28);
			this.PanDmModuleMin.TabIndex = 48;
			this.TxbDM_ModuleMax.Location = new System.Drawing.Point(247, 3);
			this.TxbDM_ModuleMax.Name = "TxbDM_ModuleMax";
			this.TxbDM_ModuleMax.Size = new System.Drawing.Size(51, 23);
			this.TxbDM_ModuleMax.TabIndex = 47;
			this.TxbDM_ModuleMax.TextChanged += new System.EventHandler(TxbDM_ModuleMax_TextChanged);
			this.TxbDM_ModuleMin.Location = new System.Drawing.Point(68, 2);
			this.TxbDM_ModuleMin.Name = "TxbDM_ModuleMin";
			this.TxbDM_ModuleMin.Size = new System.Drawing.Size(51, 23);
			this.TxbDM_ModuleMin.TabIndex = 44;
			this.TxbDM_ModuleMin.TextChanged += new System.EventHandler(TxbDM_ModuleMin_TextChanged);
			this.BtnDM_ModuleMax.Location = new System.Drawing.Point(298, 3);
			this.BtnDM_ModuleMax.Name = "BtnDM_ModuleMax";
			this.BtnDM_ModuleMax.Size = new System.Drawing.Size(49, 23);
			this.BtnDM_ModuleMax.TabIndex = 45;
			this.BtnDM_ModuleMax.Text = "确认";
			this.BtnDM_ModuleMax.UseVisualStyleBackColor = true;
			this.BtnDM_ModuleMax.Visible = false;
			this.BtnDM_ModuleMax.Click += new System.EventHandler(BtnDM_ModuleMax_Click);
			this.LabDM_ModuleMax.AutoSize = true;
			this.LabDM_ModuleMax.Location = new System.Drawing.Point(178, 6);
			this.LabDM_ModuleMax.Name = "LabDM_ModuleMax";
			this.LabDM_ModuleMax.Size = new System.Drawing.Size(70, 14);
			this.LabDM_ModuleMax.TabIndex = 46;
			this.LabDM_ModuleMax.Text = "块数最大:";
			this.BtnDM_ModuleMin.Location = new System.Drawing.Point(120, 2);
			this.BtnDM_ModuleMin.Name = "BtnDM_ModuleMin";
			this.BtnDM_ModuleMin.Size = new System.Drawing.Size(49, 23);
			this.BtnDM_ModuleMin.TabIndex = 43;
			this.BtnDM_ModuleMin.Text = "确认";
			this.BtnDM_ModuleMin.UseVisualStyleBackColor = true;
			this.BtnDM_ModuleMin.Visible = false;
			this.BtnDM_ModuleMin.Click += new System.EventHandler(BtnDM_ModuleMin_Click);
			this.LabDM_ModuleMin.AutoSize = true;
			this.LabDM_ModuleMin.Location = new System.Drawing.Point(1, 6);
			this.LabDM_ModuleMin.Name = "LabDM_ModuleMin";
			this.LabDM_ModuleMin.Size = new System.Drawing.Size(70, 14);
			this.LabDM_ModuleMin.TabIndex = 44;
			this.LabDM_ModuleMin.Text = "块数最小:";
			this.PanDmModuleSizeMin.Controls.Add(this.TxbModuleSizeMax);
			this.PanDmModuleSizeMin.Controls.Add(this.TxbModuleSizeMin);
			this.PanDmModuleSizeMin.Controls.Add(this.BtnModuleSizeMax);
			this.PanDmModuleSizeMin.Controls.Add(this.LabDMModuleSizeMax);
			this.PanDmModuleSizeMin.Controls.Add(this.BtnModuleSizeMin);
			this.PanDmModuleSizeMin.Controls.Add(this.LabDMModuleSizeMin);
			this.PanDmModuleSizeMin.Location = new System.Drawing.Point(36, 213);
			this.PanDmModuleSizeMin.Name = "PanDmModuleSizeMin";
			this.PanDmModuleSizeMin.Size = new System.Drawing.Size(353, 28);
			this.PanDmModuleSizeMin.TabIndex = 45;
			this.TxbModuleSizeMax.Location = new System.Drawing.Point(247, 3);
			this.TxbModuleSizeMax.Name = "TxbModuleSizeMax";
			this.TxbModuleSizeMax.Size = new System.Drawing.Size(51, 23);
			this.TxbModuleSizeMax.TabIndex = 47;
			this.TxbModuleSizeMax.TextChanged += new System.EventHandler(TxbModuleSizeMax_TextChanged);
			this.TxbModuleSizeMin.Location = new System.Drawing.Point(68, 2);
			this.TxbModuleSizeMin.Name = "TxbModuleSizeMin";
			this.TxbModuleSizeMin.Size = new System.Drawing.Size(51, 23);
			this.TxbModuleSizeMin.TabIndex = 44;
			this.TxbModuleSizeMin.TextChanged += new System.EventHandler(TxbModuleSizeMin_TextChanged);
			this.BtnModuleSizeMax.Location = new System.Drawing.Point(298, 3);
			this.BtnModuleSizeMax.Name = "BtnModuleSizeMax";
			this.BtnModuleSizeMax.Size = new System.Drawing.Size(49, 23);
			this.BtnModuleSizeMax.TabIndex = 45;
			this.BtnModuleSizeMax.Text = "确认";
			this.BtnModuleSizeMax.UseVisualStyleBackColor = true;
			this.BtnModuleSizeMax.Visible = false;
			this.BtnModuleSizeMax.Click += new System.EventHandler(BtnModuleSizeMax_Click);
			this.LabDMModuleSizeMax.AutoSize = true;
			this.LabDMModuleSizeMax.Location = new System.Drawing.Point(178, 6);
			this.LabDMModuleSizeMax.Name = "LabDMModuleSizeMax";
			this.LabDMModuleSizeMax.Size = new System.Drawing.Size(70, 14);
			this.LabDMModuleSizeMax.TabIndex = 46;
			this.LabDMModuleSizeMax.Text = "DM码最大:";
			this.BtnModuleSizeMin.Location = new System.Drawing.Point(120, 2);
			this.BtnModuleSizeMin.Name = "BtnModuleSizeMin";
			this.BtnModuleSizeMin.Size = new System.Drawing.Size(49, 23);
			this.BtnModuleSizeMin.TabIndex = 43;
			this.BtnModuleSizeMin.Text = "确认";
			this.BtnModuleSizeMin.UseVisualStyleBackColor = true;
			this.BtnModuleSizeMin.Visible = false;
			this.BtnModuleSizeMin.Click += new System.EventHandler(BtnModuleSizeMin_Click);
			this.LabDMModuleSizeMin.AutoSize = true;
			this.LabDMModuleSizeMin.Location = new System.Drawing.Point(1, 6);
			this.LabDMModuleSizeMin.Name = "LabDMModuleSizeMin";
			this.LabDMModuleSizeMin.Size = new System.Drawing.Size(70, 14);
			this.LabDMModuleSizeMin.TabIndex = 44;
			this.LabDMModuleSizeMin.Text = "DM码最小:";
			this.PanDmMaxGap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.PanDmMaxGap.Controls.Add(this.LabDMGapMax);
			this.PanDmMaxGap.Controls.Add(this.RdbDMGapMaxBig);
			this.PanDmMaxGap.Controls.Add(this.RdbDMGapMaxSmall);
			this.PanDmMaxGap.Controls.Add(this.RdbDMGapMaxNone);
			this.PanDmMaxGap.Location = new System.Drawing.Point(36, 61);
			this.PanDmMaxGap.Name = "PanDmMaxGap";
			this.PanDmMaxGap.Size = new System.Drawing.Size(353, 25);
			this.PanDmMaxGap.TabIndex = 44;
			this.LabDMGapMax.AutoSize = true;
			this.LabDMGapMax.Location = new System.Drawing.Point(1, 2);
			this.LabDMGapMax.Name = "LabDMGapMax";
			this.LabDMGapMax.Size = new System.Drawing.Size(140, 14);
			this.LabDMGapMax.TabIndex = 43;
			this.LabDMGapMax.Text = "DM模块允许最大空隙:";
			this.RdbDMGapMaxBig.AutoSize = true;
			this.RdbDMGapMaxBig.Location = new System.Drawing.Point(257, 1);
			this.RdbDMGapMaxBig.Name = "RdbDMGapMaxBig";
			this.RdbDMGapMaxBig.Size = new System.Drawing.Size(39, 18);
			this.RdbDMGapMaxBig.TabIndex = 40;
			this.RdbDMGapMaxBig.Text = "大";
			this.RdbDMGapMaxBig.UseVisualStyleBackColor = true;
			this.RdbDMGapMaxBig.CheckedChanged += new System.EventHandler(RdbDMGapMaxBig_CheckedChanged);
			this.RdbDMGapMaxSmall.AutoSize = true;
			this.RdbDMGapMaxSmall.Location = new System.Drawing.Point(200, 1);
			this.RdbDMGapMaxSmall.Name = "RdbDMGapMaxSmall";
			this.RdbDMGapMaxSmall.Size = new System.Drawing.Size(39, 18);
			this.RdbDMGapMaxSmall.TabIndex = 39;
			this.RdbDMGapMaxSmall.Text = "小";
			this.RdbDMGapMaxSmall.UseVisualStyleBackColor = true;
			this.RdbDMGapMaxSmall.CheckedChanged += new System.EventHandler(RdbDMGapMaxSmall_CheckedChanged);
			this.RdbDMGapMaxNone.AutoSize = true;
			this.RdbDMGapMaxNone.Location = new System.Drawing.Point(143, 1);
			this.RdbDMGapMaxNone.Name = "RdbDMGapMaxNone";
			this.RdbDMGapMaxNone.Size = new System.Drawing.Size(39, 18);
			this.RdbDMGapMaxNone.TabIndex = 38;
			this.RdbDMGapMaxNone.Text = "无";
			this.RdbDMGapMaxNone.UseVisualStyleBackColor = true;
			this.RdbDMGapMaxNone.CheckedChanged += new System.EventHandler(RdbDMGapMaxNone_CheckedChanged);
			this.ChkDMDefilementEnhance.AutoSize = true;
			this.ChkDMDefilementEnhance.Location = new System.Drawing.Point(127, 183);
			this.ChkDMDefilementEnhance.Name = "ChkDMDefilementEnhance";
			this.ChkDMDefilementEnhance.Size = new System.Drawing.Size(82, 18);
			this.ChkDMDefilementEnhance.TabIndex = 21;
			this.ChkDMDefilementEnhance.Text = "污损增强";
			this.ChkDMDefilementEnhance.UseVisualStyleBackColor = true;
			this.ChkDMDefilementEnhance.CheckedChanged += new System.EventHandler(ChkDMDefilementEnhance_CheckedChanged);
			this.ChkDMFindEnhance.AutoSize = true;
			this.ChkDMFindEnhance.Location = new System.Drawing.Point(40, 183);
			this.ChkDMFindEnhance.Name = "ChkDMFindEnhance";
			this.ChkDMFindEnhance.Size = new System.Drawing.Size(82, 18);
			this.ChkDMFindEnhance.TabIndex = 20;
			this.ChkDMFindEnhance.Text = "查找增强";
			this.ChkDMFindEnhance.UseVisualStyleBackColor = true;
			this.ChkDMFindEnhance.CheckedChanged += new System.EventHandler(ChkDMFindEnhance_CheckedChanged);
			this.PanDm2dType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.PanDm2dType.Controls.Add(this.LabDm2dType);
			this.PanDm2dType.Controls.Add(this.RdbDMShapeAll);
			this.PanDm2dType.Controls.Add(this.RdbDMShapeRectangle);
			this.PanDm2dType.Controls.Add(this.RdbDMShapeSquare);
			this.PanDm2dType.Location = new System.Drawing.Point(36, 31);
			this.PanDm2dType.Name = "PanDm2dType";
			this.PanDm2dType.Size = new System.Drawing.Size(353, 25);
			this.PanDm2dType.TabIndex = 19;
			this.LabDm2dType.AutoSize = true;
			this.LabDm2dType.Location = new System.Drawing.Point(1, 3);
			this.LabDm2dType.Name = "LabDm2dType";
			this.LabDm2dType.Size = new System.Drawing.Size(98, 14);
			this.LabDm2dType.TabIndex = 43;
			this.LabDm2dType.Text = "DM二维码类型:";
			this.RdbDMShapeAll.AutoSize = true;
			this.RdbDMShapeAll.Location = new System.Drawing.Point(105, 1);
			this.RdbDMShapeAll.Name = "RdbDMShapeAll";
			this.RdbDMShapeAll.Size = new System.Drawing.Size(81, 18);
			this.RdbDMShapeAll.TabIndex = 40;
			this.RdbDMShapeAll.Text = "任意类型";
			this.RdbDMShapeAll.UseVisualStyleBackColor = true;
			this.RdbDMShapeAll.CheckedChanged += new System.EventHandler(RdbDMShapeAll_CheckedChanged);
			this.RdbDMShapeRectangle.AutoSize = true;
			this.RdbDMShapeRectangle.Location = new System.Drawing.Point(260, 1);
			this.RdbDMShapeRectangle.Name = "RdbDMShapeRectangle";
			this.RdbDMShapeRectangle.Size = new System.Drawing.Size(53, 18);
			this.RdbDMShapeRectangle.TabIndex = 39;
			this.RdbDMShapeRectangle.Text = "矩形";
			this.RdbDMShapeRectangle.UseVisualStyleBackColor = true;
			this.RdbDMShapeRectangle.CheckedChanged += new System.EventHandler(RdbDMShapeRectangle_CheckedChanged);
			this.RdbDMShapeSquare.AutoSize = true;
			this.RdbDMShapeSquare.Location = new System.Drawing.Point(187, 1);
			this.RdbDMShapeSquare.Name = "RdbDMShapeSquare";
			this.RdbDMShapeSquare.Size = new System.Drawing.Size(67, 18);
			this.RdbDMShapeSquare.TabIndex = 38;
			this.RdbDMShapeSquare.Text = "正方形";
			this.RdbDMShapeSquare.UseVisualStyleBackColor = true;
			this.RdbDMShapeSquare.CheckedChanged += new System.EventHandler(RdbDMShapeSquare_CheckedChanged);
			this.ChbDMLowContrast.AutoSize = true;
			this.ChbDMLowContrast.Location = new System.Drawing.Point(127, 159);
			this.ChbDMLowContrast.Name = "ChbDMLowContrast";
			this.ChbDMLowContrast.Size = new System.Drawing.Size(110, 18);
			this.ChbDMLowContrast.TabIndex = 4;
			this.ChbDMLowContrast.Text = "低对比度增强";
			this.ChbDMLowContrast.UseVisualStyleBackColor = true;
			this.ChbDMLowContrast.CheckedChanged += new System.EventHandler(ChbDMLowContrast_CheckedChanged);
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			base.ClientSize = new System.Drawing.Size(414, 689);
			base.Controls.Add(this.GrbDPMSetting);
			base.Controls.Add(this.BtnOpenAllBarcodeType);
			base.Controls.Add(this.BtnOpenDefaultBarcodeType);
			base.Controls.Add(this.BtnCloseAllBarcodeType);
			base.Controls.Add(this.GrbOtherBarcode);
			base.Controls.Add(this.GrbUsually2D);
			base.Controls.Add(this.GrbUsually1D);
			this.Font = new System.Drawing.Font("宋体", 10.5f);
			base.Name = "BarcodeTypeForm";
			this.Text = "码制设置";
			base.Load += new System.EventHandler(BarcodeTypeForm_Load);
			this.GrbOtherBarcode.ResumeLayout(false);
			this.GrbOtherBarcode.PerformLayout();
			this.GrbUsually2D.ResumeLayout(false);
			this.GrbUsually2D.PerformLayout();
			this.GrbUsually1D.ResumeLayout(false);
			this.GrbUsually1D.PerformLayout();
			this.GrbDPMSetting.ResumeLayout(false);
			this.GrbDPMSetting.PerformLayout();
			this.PanDmMirror.ResumeLayout(false);
			this.PanDmMirror.PerformLayout();
			this.PanDmColor.ResumeLayout(false);
			this.PanDmColor.PerformLayout();
			this.PanDmModuleMin.ResumeLayout(false);
			this.PanDmModuleMin.PerformLayout();
			this.PanDmModuleSizeMin.ResumeLayout(false);
			this.PanDmModuleSizeMin.PerformLayout();
			this.PanDmMaxGap.ResumeLayout(false);
			this.PanDmMaxGap.PerformLayout();
			this.PanDm2dType.ResumeLayout(false);
			this.PanDm2dType.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
