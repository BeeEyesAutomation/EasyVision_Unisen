namespace BeeUi.Unit
{
    partial class AddFilters
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listAddFilter = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // listAddFilter
            // 
            this.listAddFilter.HideSelection = false;
            this.listAddFilter.Location = new System.Drawing.Point(3, 3);
            this.listAddFilter.Name = "listAddFilter";
            this.listAddFilter.Size = new System.Drawing.Size(320, 208);
            this.listAddFilter.TabIndex = 0;
            this.listAddFilter.UseCompatibleStateImageBehavior = false;
            // 
            // AddFilters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listAddFilter);
            this.Name = "AddFilters";
            this.Size = new System.Drawing.Size(367, 327);
            this.Load += new System.EventHandler(this.AddFilters_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listAddFilter;
    }
}
