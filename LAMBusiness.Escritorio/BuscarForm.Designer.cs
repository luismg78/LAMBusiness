namespace LAMBusiness.Escritorio
{
    partial class BuscarForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ProductoLabel = new System.Windows.Forms.Label();
            this.ProductoTextBox = new System.Windows.Forms.TextBox();
            this.ProductosDataGridView = new System.Windows.Forms.DataGridView();
            this.CodigoDataGridView = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataGridViewNombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataGridViewPrecioDeVenta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.NotificacionBuscarToolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.ProductosDataGridView)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProductoLabel
            // 
            this.ProductoLabel.AutoSize = true;
            this.ProductoLabel.Location = new System.Drawing.Point(12, 9);
            this.ProductoLabel.Name = "ProductoLabel";
            this.ProductoLabel.Size = new System.Drawing.Size(217, 20);
            this.ProductoLabel.TabIndex = 4;
            this.ProductoLabel.Text = "Código o nombre del producto";
            // 
            // ProductoTextBox
            // 
            this.ProductoTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProductoTextBox.Location = new System.Drawing.Point(12, 32);
            this.ProductoTextBox.Name = "ProductoTextBox";
            this.ProductoTextBox.Size = new System.Drawing.Size(772, 27);
            this.ProductoTextBox.TabIndex = 5;
            this.ProductoTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProductoTextBox_KeyDown);
            // 
            // ProductosDataGridView
            // 
            this.ProductosDataGridView.AllowUserToAddRows = false;
            this.ProductosDataGridView.AllowUserToDeleteRows = false;
            this.ProductosDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProductosDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ProductosDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.ProductosDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.ProductosDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            this.ProductosDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProductosDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CodigoDataGridView,
            this.DataGridViewNombre,
            this.DataGridViewPrecioDeVenta});
            this.ProductosDataGridView.Location = new System.Drawing.Point(12, 65);
            this.ProductosDataGridView.MultiSelect = false;
            this.ProductosDataGridView.Name = "ProductosDataGridView";
            this.ProductosDataGridView.ReadOnly = true;
            this.ProductosDataGridView.RowHeadersWidth = 51;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(65)))), ((int)(((byte)(82)))));
            this.ProductosDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ProductosDataGridView.RowTemplate.Height = 29;
            this.ProductosDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ProductosDataGridView.Size = new System.Drawing.Size(772, 423);
            this.ProductosDataGridView.TabIndex = 6;
            this.ProductosDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProductosDataGridView_KeyDown);
            // 
            // CodigoDataGridView
            // 
            this.CodigoDataGridView.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CodigoDataGridView.HeaderText = "Código";
            this.CodigoDataGridView.MinimumWidth = 6;
            this.CodigoDataGridView.Name = "CodigoDataGridView";
            this.CodigoDataGridView.ReadOnly = true;
            this.CodigoDataGridView.Width = 87;
            // 
            // DataGridViewNombre
            // 
            this.DataGridViewNombre.HeaderText = "Nombre";
            this.DataGridViewNombre.MinimumWidth = 6;
            this.DataGridViewNombre.Name = "DataGridViewNombre";
            this.DataGridViewNombre.ReadOnly = true;
            // 
            // DataGridViewPrecioDeVenta
            // 
            this.DataGridViewPrecioDeVenta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DataGridViewPrecioDeVenta.HeaderText = "Precio";
            this.DataGridViewPrecioDeVenta.MinimumWidth = 6;
            this.DataGridViewPrecioDeVenta.Name = "DataGridViewPrecioDeVenta";
            this.DataGridViewPrecioDeVenta.ReadOnly = true;
            this.DataGridViewPrecioDeVenta.Width = 79;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NotificacionBuscarToolStripStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 513);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(796, 26);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // NotificacionBuscarToolStripStatus
            // 
            this.NotificacionBuscarToolStripStatus.Font = new System.Drawing.Font("Segoe UI Light", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.NotificacionBuscarToolStripStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.NotificacionBuscarToolStripStatus.Name = "NotificacionBuscarToolStripStatus";
            this.NotificacionBuscarToolStripStatus.Size = new System.Drawing.Size(781, 20);
            this.NotificacionBuscarToolStripStatus.Spring = true;
            this.NotificacionBuscarToolStripStatus.Text = "Notificación";
            this.NotificacionBuscarToolStripStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BuscarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 539);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.ProductosDataGridView);
            this.Controls.Add(this.ProductoTextBox);
            this.Controls.Add(this.ProductoLabel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BuscarForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Buscar producto";
            this.Load += new System.EventHandler(this.BuscarForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ProductosDataGridView)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label ProductoLabel;
        private TextBox ProductoTextBox;
        private DataGridView ProductosDataGridView;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel NotificacionBuscarToolStripStatus;
        private DataGridViewTextBoxColumn CodigoDataGridView;
        private DataGridViewTextBoxColumn DataGridViewNombre;
        private DataGridViewTextBoxColumn DataGridViewPrecioDeVenta;
    }
}