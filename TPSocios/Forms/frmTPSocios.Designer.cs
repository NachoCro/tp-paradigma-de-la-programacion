namespace TPSocios.Forms
{
    partial class frmTPSocios
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.gbxDatos = new System.Windows.Forms.GroupBox();
            this.chkDisponible = new System.Windows.Forms.CheckBox();
            this.cmbTipoSocio = new System.Windows.Forms.ComboBox();
            this.txtCuotaMensual = new System.Windows.Forms.TextBox();
            this.lblCuotaMensual = new System.Windows.Forms.Label();
            this.dtpFechaNacimiento = new System.Windows.Forms.DateTimePicker();
            this.lblFechaNacimiento = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtApellido = new System.Windows.Forms.TextBox();
            this.lblApellido = new System.Windows.Forms.Label();
            this.mtxtLegajoSocio = new System.Windows.Forms.MaskedTextBox();
            this.lblLegajoSocio = new System.Windows.Forms.Label();
            this.dgvSocios = new System.Windows.Forms.DataGridView();
            this.lblAyuda = new System.Windows.Forms.Label();
            this.lblIntegrantes = new System.Windows.Forms.Label();
            this.explosionErrores = new TPSocios.Presentacion.Animaciones.ExplosionTexto();
            this.btnRegistrar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.gbxDatos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSocios)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxDatos
            // 
            this.gbxDatos.Controls.Add(this.chkDisponible);
            this.gbxDatos.Controls.Add(this.cmbTipoSocio);
            this.gbxDatos.Controls.Add(this.txtCuotaMensual);
            this.gbxDatos.Controls.Add(this.lblCuotaMensual);
            this.gbxDatos.Controls.Add(this.dtpFechaNacimiento);
            this.gbxDatos.Controls.Add(this.lblFechaNacimiento);
            this.gbxDatos.Controls.Add(this.txtEmail);
            this.gbxDatos.Controls.Add(this.lblEmail);
            this.gbxDatos.Controls.Add(this.txtNombre);
            this.gbxDatos.Controls.Add(this.lblNombre);
            this.gbxDatos.Controls.Add(this.txtApellido);
            this.gbxDatos.Controls.Add(this.lblApellido);
            this.gbxDatos.Controls.Add(this.mtxtLegajoSocio);
            this.gbxDatos.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.gbxDatos.Controls.Add(this.lblLegajoSocio);
            this.gbxDatos.Location = new System.Drawing.Point(12, 12);
            this.gbxDatos.Name = "gbxDatos";
            this.gbxDatos.Size = new System.Drawing.Size(560, 250);
            this.gbxDatos.TabIndex = 0;
            this.gbxDatos.TabStop = false;
            this.gbxDatos.Text = "Datos del Socio";
            // 
            // lblLegajoSocio
            // 
            this.lblLegajoSocio.AutoSize = true;
            this.lblLegajoSocio.Location = new System.Drawing.Point(20, 35);
            this.lblLegajoSocio.Name = "lblLegajoSocio";
            this.lblLegajoSocio.Size = new System.Drawing.Size(76, 15);
            this.lblLegajoSocio.TabIndex = 0;
            this.lblLegajoSocio.Text = "Legajo Socio:";
            // 
            // mtxtLegajoSocio
            // 
            this.mtxtLegajoSocio.Location = new System.Drawing.Point(150, 32);
            this.mtxtLegajoSocio.Name = "mtxtLegajoSocio";
            this.mtxtLegajoSocio.Size = new System.Drawing.Size(120, 23);
            this.mtxtLegajoSocio.TabIndex = 1;
            this.mtxtLegajoSocio.Mask = "?\\-####";
            // 
            // lblApellido
            // 
            this.lblApellido.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.lblApellido.AutoSize = true;
            this.lblApellido.Location = new System.Drawing.Point(300, 35);
            this.lblApellido.Name = "lblApellido";
            this.lblApellido.Size = new System.Drawing.Size(60, 15);
            this.lblApellido.TabIndex = 2;
            this.lblApellido.Text = "Apellido:";
            // 
            // txtApellido
            // 
            this.txtApellido.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.txtApellido.Location = new System.Drawing.Point(370, 32);
            this.txtApellido.MaxLength = 50;
            this.txtApellido.Name = "txtApellido";
            this.txtApellido.Size = new System.Drawing.Size(160, 23);
            this.txtApellido.TabIndex = 3;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(20, 70);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(53, 15);
            this.lblNombre.TabIndex = 4;
            this.lblNombre.Text = "Nombre:";
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(150, 67);
            this.txtNombre.MaxLength = 50;
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(120, 23);
            this.txtNombre.TabIndex = 5;
            // 
            // lblEmail
            // 
            this.lblEmail.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(300, 70);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(40, 15);
            this.lblEmail.TabIndex = 6;
            this.lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            this.txtEmail.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.txtEmail.Location = new System.Drawing.Point(370, 67);
            this.txtEmail.MaxLength = 100;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(160, 23);
            this.txtEmail.TabIndex = 7;
            // 
            // lblFechaNacimiento
            // 
            this.lblFechaNacimiento.AutoSize = true;
            this.lblFechaNacimiento.Location = new System.Drawing.Point(20, 105);
            this.lblFechaNacimiento.Name = "lblFechaNacimiento";
            this.lblFechaNacimiento.Size = new System.Drawing.Size(113, 15);
            this.lblFechaNacimiento.TabIndex = 8;
            this.lblFechaNacimiento.Text = "Fecha de Nacimiento:";
            // 
            // dtpFechaNacimiento
            // 
            this.dtpFechaNacimiento.Location = new System.Drawing.Point(150, 102);
            this.dtpFechaNacimiento.CustomFormat = "dd/MM/yyyy";
            this.dtpFechaNacimiento.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFechaNacimiento.MaxDate = new System.DateTime(9998, 12, 31);
            this.dtpFechaNacimiento.Name = "dtpFechaNacimiento";
            this.dtpFechaNacimiento.Size = new System.Drawing.Size(120, 23);
            this.dtpFechaNacimiento.TabIndex = 9;
            // 
            // lblCuotaMensual
            // 
            this.lblCuotaMensual.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.lblCuotaMensual.AutoSize = true;
            this.lblCuotaMensual.Location = new System.Drawing.Point(300, 105);
            this.lblCuotaMensual.Name = "lblCuotaMensual";
            this.lblCuotaMensual.Size = new System.Drawing.Size(96, 15);
            this.lblCuotaMensual.TabIndex = 10;
            this.lblCuotaMensual.Text = "Cuota Mensual:";
            // 
            // txtCuotaMensual
            // 
            this.txtCuotaMensual.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.txtCuotaMensual.Location = new System.Drawing.Point(410, 102);
            this.txtCuotaMensual.Name = "txtCuotaMensual";
            this.txtCuotaMensual.Size = new System.Drawing.Size(120, 23);
            this.txtCuotaMensual.TabIndex = 11;
            this.txtCuotaMensual.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cmbTipoSocio
            // 
            this.cmbTipoSocio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoSocio.FormattingEnabled = true;
            this.cmbTipoSocio.Location = new System.Drawing.Point(150, 137);
            this.cmbTipoSocio.Name = "cmbTipoSocio";
            this.cmbTipoSocio.Size = new System.Drawing.Size(190, 23);
            this.cmbTipoSocio.TabIndex = 12;
            // 
            // chkDisponible
            // 
            this.chkDisponible.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.chkDisponible.AutoSize = false;
            this.chkDisponible.Location = new System.Drawing.Point(348, 141);
            this.chkDisponible.Name = "chkDisponible";
            this.chkDisponible.Size = new System.Drawing.Size(96, 23);
            this.chkDisponible.TabIndex = 13;
            this.chkDisponible.Text = "Disponible";
            this.chkDisponible.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkDisponible.UseVisualStyleBackColor = true;
            // 
            // dgvSocios
            // 
            this.dgvSocios.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvSocios.AllowUserToAddRows = false;
            this.dgvSocios.AllowUserToDeleteRows = false;
            this.dgvSocios.AllowUserToResizeRows = false;
            this.dgvSocios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSocios.Location = new System.Drawing.Point(12, 272);
            this.dgvSocios.MultiSelect = false;
            this.dgvSocios.Name = "dgvSocios";
            this.dgvSocios.ReadOnly = true;
            this.dgvSocios.RowHeadersVisible = false;
            this.dgvSocios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSocios.Size = new System.Drawing.Size(560, 188);
            this.dgvSocios.TabIndex = 14;
            this.dgvSocios.SelectionChanged += new System.EventHandler(this.dgvSocios_SelectionChanged);
            // 
            // lblAyuda
            // 
            this.lblAyuda.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.lblAyuda.AutoSize = false;
            this.lblAyuda.Location = new System.Drawing.Point(12, 534);
            this.lblAyuda.Name = "lblAyuda";
            this.lblAyuda.Size = new System.Drawing.Size(330, 50);
            this.lblAyuda.TabIndex = 15;
            this.lblAyuda.Text = "Seleccione una fila para modificarla. Con la fila elegida, presione Supr para eliminarla.";
            // 
            // lblIntegrantes
            // 
            this.lblIntegrantes.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.lblIntegrantes.AutoSize = false;
            this.lblIntegrantes.Location = new System.Drawing.Point(12, 600);
            this.lblIntegrantes.Name = "lblIntegrantes";
            this.lblIntegrantes.Size = new System.Drawing.Size(560, 18);
            this.lblIntegrantes.TabIndex = 20;
            this.lblIntegrantes.Text = "Integrantes: Fabricio Gullino - Ignacio Crocetti";
            this.lblIntegrantes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // explosionErrores
            // 
            this.explosionErrores.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.explosionErrores.Location = new System.Drawing.Point(12, 466);
            this.explosionErrores.Name = "explosionErrores";
            this.explosionErrores.Size = new System.Drawing.Size(560, 60);
            this.explosionErrores.TabIndex = 19;
            // 
            // btnRegistrar
            // 
            this.btnRegistrar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.btnRegistrar.Location = new System.Drawing.Point(428, 550);
            this.btnRegistrar.Name = "btnRegistrar";
            this.btnRegistrar.Size = new System.Drawing.Size(70, 27);
            this.btnRegistrar.TabIndex = 16;
            this.btnRegistrar.Text = "Registrar";
            this.btnRegistrar.UseVisualStyleBackColor = true;
            this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.btnCancelar.Location = new System.Drawing.Point(502, 550);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(70, 27);
            this.btnCancelar.TabIndex = 18;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // frmTPSocios
            // 
            this.AcceptButton = this.btnRegistrar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(584, 630);
            this.Controls.Add(this.explosionErrores);
            this.Controls.Add(this.btnRegistrar);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.lblIntegrantes);
            this.Controls.Add(this.lblAyuda);
            this.Controls.Add(this.dgvSocios);
            this.Controls.Add(this.gbxDatos);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTPSocios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Club Social Deportivo";
            this.Load += new System.EventHandler(this.frmTPSocios_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmTPSocios_KeyDown);
            this.gbxDatos.ResumeLayout(false);
            this.gbxDatos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSocios)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox gbxDatos;
        private System.Windows.Forms.Label lblLegajoSocio;
        private System.Windows.Forms.MaskedTextBox mtxtLegajoSocio;
        private System.Windows.Forms.Label lblApellido;
        private System.Windows.Forms.TextBox txtApellido;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblFechaNacimiento;
        private System.Windows.Forms.DateTimePicker dtpFechaNacimiento;
        private System.Windows.Forms.Label lblCuotaMensual;
        private System.Windows.Forms.TextBox txtCuotaMensual;
        private System.Windows.Forms.ComboBox cmbTipoSocio;
        private System.Windows.Forms.CheckBox chkDisponible;
        private System.Windows.Forms.DataGridView dgvSocios;
        private System.Windows.Forms.Label lblAyuda;
        private System.Windows.Forms.Label lblIntegrantes;
        private TPSocios.Presentacion.Animaciones.ExplosionTexto explosionErrores;
        private System.Windows.Forms.Button btnRegistrar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
