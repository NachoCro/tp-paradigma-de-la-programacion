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
            gbxDatos = new GroupBox();
            chkDisponible = new CheckBox();
            cmbTipoSocio = new ComboBox();
            txtCuotaMensual = new TextBox();
            lblCuotaMensual = new Label();
            dtpFechaNacimiento = new DateTimePicker();
            lblFechaNacimiento = new Label();
            txtEmail = new TextBox();
            lblEmail = new Label();
            txtNombre = new TextBox();
            lblNombre = new Label();
            txtApellido = new TextBox();
            lblApellido = new Label();
            mtxtLegajoSocio = new MaskedTextBox();
            lblLegajoSocio = new Label();
            dgvSocios = new DataGridView();
            lblAyuda = new Label();
            lblIntegrantes = new Label();
            btnRegistrar = new Button();
            btnCancelar = new Button();
            mensajeErrores = new TPSocios.Presentacion.Animaciones.TextoEscribiendo();
            pbOperacion = new TPSocios.Presentacion.Animaciones.BarraCarga();
            gbxDatos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSocios).BeginInit();
            SuspendLayout();
            // 
            // gbxDatos
            // 
            gbxDatos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            gbxDatos.Controls.Add(chkDisponible);
            gbxDatos.Controls.Add(cmbTipoSocio);
            gbxDatos.Controls.Add(txtCuotaMensual);
            gbxDatos.Controls.Add(lblCuotaMensual);
            gbxDatos.Controls.Add(dtpFechaNacimiento);
            gbxDatos.Controls.Add(lblFechaNacimiento);
            gbxDatos.Controls.Add(txtEmail);
            gbxDatos.Controls.Add(lblEmail);
            gbxDatos.Controls.Add(txtNombre);
            gbxDatos.Controls.Add(lblNombre);
            gbxDatos.Controls.Add(txtApellido);
            gbxDatos.Controls.Add(lblApellido);
            gbxDatos.Controls.Add(mtxtLegajoSocio);
            gbxDatos.Controls.Add(lblLegajoSocio);
            gbxDatos.Location = new Point(14, 16);
            gbxDatos.Margin = new Padding(3, 4, 3, 4);
            gbxDatos.Name = "gbxDatos";
            gbxDatos.Padding = new Padding(3, 4, 3, 4);
            gbxDatos.Size = new Size(640, 333);
            gbxDatos.TabIndex = 0;
            gbxDatos.TabStop = false;
            gbxDatos.Text = "Datos del Socio";
            // 
            // chkDisponible
            // 
            chkDisponible.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkDisponible.Location = new Point(343, 185);
            chkDisponible.Margin = new Padding(3, 4, 3, 4);
            chkDisponible.Name = "chkDisponible";
            chkDisponible.Size = new Size(110, 31);
            chkDisponible.TabIndex = 13;
            chkDisponible.Text = "Disponible";
            chkDisponible.UseVisualStyleBackColor = true;
            // 
            // cmbTipoSocio
            // 
            cmbTipoSocio.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipoSocio.FormattingEnabled = true;
            cmbTipoSocio.Location = new Point(91, 186);
            cmbTipoSocio.Margin = new Padding(3, 4, 3, 4);
            cmbTipoSocio.Name = "cmbTipoSocio";
            cmbTipoSocio.Size = new Size(217, 28);
            cmbTipoSocio.TabIndex = 12;
            // 
            // txtCuotaMensual
            // 
            txtCuotaMensual.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtCuotaMensual.Location = new Point(469, 136);
            txtCuotaMensual.Margin = new Padding(3, 4, 3, 4);
            txtCuotaMensual.Name = "txtCuotaMensual";
            txtCuotaMensual.Size = new Size(137, 27);
            txtCuotaMensual.TabIndex = 11;
            txtCuotaMensual.TextAlign = HorizontalAlignment.Right;
            // 
            // lblCuotaMensual
            // 
            lblCuotaMensual.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblCuotaMensual.AutoSize = true;
            lblCuotaMensual.Location = new Point(343, 140);
            lblCuotaMensual.Name = "lblCuotaMensual";
            lblCuotaMensual.Size = new Size(110, 20);
            lblCuotaMensual.TabIndex = 10;
            lblCuotaMensual.Text = "Cuota Mensual:";
            // 
            // dtpFechaNacimiento
            // 
            dtpFechaNacimiento.CustomFormat = "dd/MM/yyyy";
            dtpFechaNacimiento.Format = DateTimePickerFormat.Custom;
            dtpFechaNacimiento.Location = new Point(171, 136);
            dtpFechaNacimiento.Margin = new Padding(3, 4, 3, 4);
            dtpFechaNacimiento.Name = "dtpFechaNacimiento";
            dtpFechaNacimiento.Size = new Size(137, 27);
            dtpFechaNacimiento.TabIndex = 9;
            // 
            // lblFechaNacimiento
            // 
            lblFechaNacimiento.AutoSize = true;
            lblFechaNacimiento.Location = new Point(23, 140);
            lblFechaNacimiento.Name = "lblFechaNacimiento";
            lblFechaNacimiento.Size = new Size(152, 20);
            lblFechaNacimiento.TabIndex = 8;
            lblFechaNacimiento.Text = "Fecha de Nacimiento:";
            // 
            // txtEmail
            // 
            txtEmail.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtEmail.Location = new Point(423, 89);
            txtEmail.Margin = new Padding(3, 4, 3, 4);
            txtEmail.MaxLength = 100;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(182, 27);
            txtEmail.TabIndex = 7;
            // 
            // lblEmail
            // 
            lblEmail.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(343, 93);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(49, 20);
            lblEmail.TabIndex = 6;
            lblEmail.Text = "Email:";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(171, 89);
            txtNombre.Margin = new Padding(3, 4, 3, 4);
            txtNombre.MaxLength = 50;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(137, 27);
            txtNombre.TabIndex = 5;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(23, 93);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(67, 20);
            lblNombre.TabIndex = 4;
            lblNombre.Text = "Nombre:";
            // 
            // txtApellido
            // 
            txtApellido.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtApellido.Location = new Point(423, 43);
            txtApellido.Margin = new Padding(3, 4, 3, 4);
            txtApellido.MaxLength = 50;
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(182, 27);
            txtApellido.TabIndex = 3;
            // 
            // lblApellido
            // 
            lblApellido.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblApellido.AutoSize = true;
            lblApellido.Location = new Point(343, 47);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(69, 20);
            lblApellido.TabIndex = 2;
            lblApellido.Text = "Apellido:";
            // 
            // mtxtLegajoSocio
            // 
            mtxtLegajoSocio.Location = new Point(171, 43);
            mtxtLegajoSocio.Margin = new Padding(3, 4, 3, 4);
            mtxtLegajoSocio.Mask = "?\\-####";
            mtxtLegajoSocio.Name = "mtxtLegajoSocio";
            mtxtLegajoSocio.Size = new Size(137, 27);
            mtxtLegajoSocio.TabIndex = 1;
            // 
            // lblLegajoSocio
            // 
            lblLegajoSocio.AutoSize = true;
            lblLegajoSocio.Location = new Point(23, 47);
            lblLegajoSocio.Name = "lblLegajoSocio";
            lblLegajoSocio.Size = new Size(98, 20);
            lblLegajoSocio.TabIndex = 0;
            lblLegajoSocio.Text = "Legajo Socio:";
            // 
            // dgvSocios
            // 
            dgvSocios.AllowUserToAddRows = false;
            dgvSocios.AllowUserToDeleteRows = false;
            dgvSocios.AllowUserToResizeRows = false;
            dgvSocios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvSocios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSocios.Location = new Point(14, 363);
            dgvSocios.Margin = new Padding(3, 4, 3, 4);
            dgvSocios.MultiSelect = false;
            dgvSocios.Name = "dgvSocios";
            dgvSocios.ReadOnly = true;
            dgvSocios.RowHeadersVisible = false;
            dgvSocios.RowHeadersWidth = 51;
            dgvSocios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSocios.Size = new Size(640, 251);
            dgvSocios.TabIndex = 14;
            dgvSocios.SelectionChanged += dgvSocios_SelectionChanged;
            // 
            // lblAyuda
            // 
            lblAyuda.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblAyuda.Location = new Point(14, 712);
            lblAyuda.Name = "lblAyuda";
            lblAyuda.Size = new Size(377, 67);
            lblAyuda.TabIndex = 15;
            lblAyuda.Text = "Seleccione una fila para modificarla. Con la fila elegida, presione Supr para eliminarla.";
            // 
            // lblIntegrantes
            // 
            lblIntegrantes.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblIntegrantes.Location = new Point(14, 800);
            lblIntegrantes.Name = "lblIntegrantes";
            lblIntegrantes.Size = new Size(640, 24);
            lblIntegrantes.TabIndex = 20;
            lblIntegrantes.Text = "Integrantes: Fabricio Gullino - Ignacio Crocetti";
            lblIntegrantes.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // mensajeErrores
            // 
            mensajeErrores.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            mensajeErrores.Location = new Point(14, 621);
            mensajeErrores.Margin = new Padding(3, 4, 3, 4);
            mensajeErrores.Name = "mensajeErrores";
            mensajeErrores.Size = new Size(640, 80);
            mensajeErrores.TabIndex = 19;
            mensajeErrores.TabStop = false;
            mensajeErrores.Visible = false;
            // 
            // btnRegistrar
            // 
            btnRegistrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRegistrar.Location = new Point(489, 733);
            btnRegistrar.Margin = new Padding(3, 4, 3, 4);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(80, 36);
            btnRegistrar.TabIndex = 16;
            btnRegistrar.Text = "Registrar";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancelar.Location = new Point(574, 733);
            btnCancelar.Margin = new Padding(3, 4, 3, 4);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(80, 36);
            btnCancelar.TabIndex = 18;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // pbOperacion
            // 
            pbOperacion.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pbOperacion.Location = new Point(14, 781);
            pbOperacion.Margin = new Padding(3, 4, 3, 4);
            pbOperacion.Name = "pbOperacion";
            pbOperacion.Size = new Size(640, 16);
            pbOperacion.TabIndex = 21;
            pbOperacion.TabStop = false;
            pbOperacion.Visible = false;
            // 
            // frmTPSocios
            // 
            AcceptButton = btnRegistrar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(667, 840);
            Controls.Add(mensajeErrores);
            Controls.Add(btnRegistrar);
            Controls.Add(btnCancelar);
            Controls.Add(lblIntegrantes);
            Controls.Add(pbOperacion);
            Controls.Add(lblAyuda);
            Controls.Add(dgvSocios);
            Controls.Add(gbxDatos);
            KeyPreview = true;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmTPSocios";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Club Social Deportivo";
            Load += frmTPSocios_Load;
            KeyDown += frmTPSocios_KeyDown;
            gbxDatos.ResumeLayout(false);
            gbxDatos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSocios).EndInit();
            ResumeLayout(false);
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
        private TPSocios.Presentacion.Animaciones.TextoEscribiendo mensajeErrores;
        private System.Windows.Forms.Button btnRegistrar;
        private System.Windows.Forms.Button btnCancelar;
        private TPSocios.Presentacion.Animaciones.BarraCarga pbOperacion;
    }
}
