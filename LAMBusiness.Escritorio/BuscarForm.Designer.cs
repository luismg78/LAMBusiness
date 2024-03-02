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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            ProductoLabel = new Label();
            ProductoTextBox = new TextBox();
            ProductosDataGridView = new DataGridView();
            CodigoDataGridView = new DataGridViewTextBoxColumn();
            DataGridViewNombre = new DataGridViewTextBoxColumn();
            DataGridViewPrecioDeVenta = new DataGridViewTextBoxColumn();
            statusStrip1 = new StatusStrip();
            NotificacionBuscarToolStripStatus = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)ProductosDataGridView).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // ProductoLabel
            // 
            ProductoLabel.AutoSize = true;
            ProductoLabel.Location = new Point(10, 7);
            ProductoLabel.Name = "ProductoLabel";
            ProductoLabel.Size = new Size(172, 15);
            ProductoLabel.TabIndex = 4;
            ProductoLabel.Text = "Código o nombre del producto";
            // 
            // ProductoTextBox
            // 
            ProductoTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ProductoTextBox.Location = new Point(10, 24);
            ProductoTextBox.Margin = new Padding(3, 2, 3, 2);
            ProductoTextBox.Name = "ProductoTextBox";
            ProductoTextBox.Size = new Size(676, 23);
            ProductoTextBox.TabIndex = 5;
            ProductoTextBox.KeyDown += ProductoTextBox_KeyDown;
            // 
            // ProductosDataGridView
            // 
            ProductosDataGridView.AllowUserToAddRows = false;
            ProductosDataGridView.AllowUserToDeleteRows = false;
            ProductosDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ProductosDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ProductosDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            ProductosDataGridView.BackgroundColor = Color.White;
            ProductosDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;
            ProductosDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ProductosDataGridView.Columns.AddRange(new DataGridViewColumn[] { CodigoDataGridView, DataGridViewNombre, DataGridViewPrecioDeVenta });
            ProductosDataGridView.Location = new Point(10, 49);
            ProductosDataGridView.Margin = new Padding(3, 2, 3, 2);
            ProductosDataGridView.MultiSelect = false;
            ProductosDataGridView.Name = "ProductosDataGridView";
            ProductosDataGridView.ReadOnly = true;
            ProductosDataGridView.RowHeadersWidth = 51;
            dataGridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 12F);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(255, 65, 82);
            ProductosDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle1;
            ProductosDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ProductosDataGridView.Size = new Size(676, 317);
            ProductosDataGridView.TabIndex = 6;
            ProductosDataGridView.DoubleClick += ProductosDataGridView_DoubleClick;
            ProductosDataGridView.KeyDown += ProductosDataGridView_KeyDown;
            // 
            // CodigoDataGridView
            // 
            CodigoDataGridView.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            CodigoDataGridView.HeaderText = "Código";
            CodigoDataGridView.MinimumWidth = 6;
            CodigoDataGridView.Name = "CodigoDataGridView";
            CodigoDataGridView.ReadOnly = true;
            CodigoDataGridView.Width = 71;
            // 
            // DataGridViewNombre
            // 
            DataGridViewNombre.HeaderText = "Nombre";
            DataGridViewNombre.MinimumWidth = 6;
            DataGridViewNombre.Name = "DataGridViewNombre";
            DataGridViewNombre.ReadOnly = true;
            // 
            // DataGridViewPrecioDeVenta
            // 
            DataGridViewPrecioDeVenta.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DataGridViewPrecioDeVenta.HeaderText = "Precio";
            DataGridViewPrecioDeVenta.MinimumWidth = 6;
            DataGridViewPrecioDeVenta.Name = "DataGridViewPrecioDeVenta";
            DataGridViewPrecioDeVenta.ReadOnly = true;
            DataGridViewPrecioDeVenta.Width = 65;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { NotificacionBuscarToolStripStatus });
            statusStrip1.Location = new Point(0, 382);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 12, 0);
            statusStrip1.Size = new Size(696, 22);
            statusStrip1.TabIndex = 7;
            statusStrip1.Text = "statusStrip1";
            // 
            // NotificacionBuscarToolStripStatus
            // 
            NotificacionBuscarToolStripStatus.Font = new Font("Segoe UI Light", 9F);
            NotificacionBuscarToolStripStatus.ForeColor = Color.FromArgb(192, 0, 0);
            NotificacionBuscarToolStripStatus.Name = "NotificacionBuscarToolStripStatus";
            NotificacionBuscarToolStripStatus.Size = new Size(683, 17);
            NotificacionBuscarToolStripStatus.Spring = true;
            NotificacionBuscarToolStripStatus.Text = "Notificación";
            NotificacionBuscarToolStripStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // BuscarForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(696, 404);
            Controls.Add(statusStrip1);
            Controls.Add(ProductosDataGridView);
            Controls.Add(ProductoTextBox);
            Controls.Add(ProductoLabel);
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "BuscarForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Buscar producto";
            Load += BuscarForm_Load;
            ((System.ComponentModel.ISupportInitialize)ProductosDataGridView).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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