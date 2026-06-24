Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Windows.Forms

''' <summary>
''' Módulo central de conexión (Servidor: DESKTOP-KSF675V\SQLEXPRESS)
''' Como es Público, el archivo panel.vb también podrá usarlo sin problemas.
''' </summary>
Public Module DatabaseConnection
    Public Const CadenaConexion As String = "Data Source=.\SQLEXPRESS;Initial Catalog=SpotiCloud;Integrated Security=True"
    Public Function GetConnection() As SqlConnection
        Dim conexion As New SqlConnection(CadenaConexion)
        conexion.Open()
        Return conexion
    End Function
End Module

' =========================================================================================
' CLASE 1: INICIO DE SESIÓN
' =========================================================================================
Partial Public Class Form1
    Inherits Form

    Private txtUsuario As New TextBox()
    Private txtPassword As New TextBox()
    Private WithEvents btnLogin As New Button()
    Private WithEvents btnRegistrar As New Button()
    Private WithEvents btnSalir As New Button()

    Public Sub New()
        InitializeComponent()
        Me.AutoScaleMode = AutoScaleMode.None
        Me.Text = "SpotiCloud - Iniciar Sesión"
        Me.Size = New Size(500, 800)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.FromArgb(18, 18, 18)
        Me.ForeColor = Color.White
        Me.Font = New Font("Segoe UI", 10.0F)
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        ConfigurarLoginUI()
    End Sub

    Private Sub ConfigurarLoginUI()
        Dim lblLogo As New Label() With {.Text = "SpotiCloud", .Bounds = New Rectangle(0, 60, 500, 100), .Font = New Font("Segoe UI Black", 36.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(30, 215, 96), .TextAlign = ContentAlignment.MiddleCenter}
        Dim lblSubtitulo As New Label() With {.Text = "Música para todos. Inicia sesión para continuar.", .Bounds = New Rectangle(0, 160, 500, 30), .Font = New Font("Segoe UI", 11.0F), .ForeColor = Color.LightGray, .TextAlign = ContentAlignment.MiddleCenter}

        Dim lblUser As New Label() With {.Text = "Nombre de usuario", .Bounds = New Rectangle(85, 240, 330, 25), .Font = New Font("Segoe UI", 11.0F, FontStyle.Bold)}
        txtUsuario.AutoSize = False
        txtUsuario.Bounds = New Rectangle(85, 275, 330, 45)
        txtUsuario.Font = New Font("Segoe UI", 14.0F)
        txtUsuario.BackColor = Color.FromArgb(40, 40, 40)
        txtUsuario.ForeColor = Color.White
        txtUsuario.BorderStyle = BorderStyle.FixedSingle

        Dim lblPass As New Label() With {.Text = "Contraseña", .Bounds = New Rectangle(85, 350, 330, 25), .Font = New Font("Segoe UI", 11.0F, FontStyle.Bold)}
        txtPassword.AutoSize = False
        txtPassword.Bounds = New Rectangle(85, 385, 330, 45)
        txtPassword.Font = New Font("Segoe UI", 14.0F)
        txtPassword.BackColor = Color.FromArgb(40, 40, 40)
        txtPassword.ForeColor = Color.White
        txtPassword.BorderStyle = BorderStyle.FixedSingle
        txtPassword.PasswordChar = "•"c

        btnLogin.Text = "INICIAR SESIÓN"
        btnLogin.Bounds = New Rectangle(85, 490, 330, 55)
        btnLogin.BackColor = Color.FromArgb(30, 215, 96)
        btnLogin.ForeColor = Color.Black
        btnLogin.FlatStyle = FlatStyle.Flat
        btnLogin.FlatAppearance.BorderSize = 0
        btnLogin.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        btnLogin.Cursor = Cursors.Hand

        btnRegistrar.Text = "REGISTRARSE"
        btnRegistrar.Bounds = New Rectangle(85, 570, 330, 55)
        btnRegistrar.BackColor = Color.FromArgb(45, 45, 45)
        btnRegistrar.ForeColor = Color.White
        btnRegistrar.FlatStyle = FlatStyle.Flat
        btnRegistrar.FlatAppearance.BorderSize = 0
        btnRegistrar.Font = New Font("Segoe UI", 11.0F, FontStyle.Bold)
        btnRegistrar.Cursor = Cursors.Hand

        btnSalir.Text = "Salir de la aplicación"
        btnSalir.Bounds = New Rectangle(85, 660, 330, 35)
        btnSalir.BackColor = Color.Transparent
        btnSalir.ForeColor = Color.DarkGray
        btnSalir.FlatStyle = FlatStyle.Flat
        btnSalir.FlatAppearance.BorderSize = 0
        btnSalir.Font = New Font("Segoe UI", 10.0F, FontStyle.Underline)
        btnSalir.Cursor = Cursors.Hand

        Me.Controls.Add(lblLogo) : Me.Controls.Add(lblSubtitulo) : Me.Controls.Add(lblUser)
        Me.Controls.Add(txtUsuario) : Me.Controls.Add(lblPass) : Me.Controls.Add(txtPassword)
        Me.Controls.Add(btnLogin) : Me.Controls.Add(btnRegistrar) : Me.Controls.Add(btnSalir)
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim userIngresado As String = txtUsuario.Text.Trim()
        Dim passIngresado As String = txtPassword.Text.Trim()

        If userIngresado = "admin" AndAlso passIngresado = "admin" Then
            Dim panelAdmin As New frmAdmin()
            panelAdmin.Show()
            Me.Hide()
            Return
        End If

        Dim nombreReal As String = ""
        Try
            Using con = GetConnection()
                Dim query As String = "SELECT Nombre FROM Usuarios WHERE (Email = @User OR Nombre = @User) AND Contrasena = @Pass"
                Using cmd As New SqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@User", userIngresado)
                    cmd.Parameters.AddWithValue("@Pass", passIngresado)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing Then nombreReal = result.ToString()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error de BD: " & ex.Message, "Error") : Return
        End Try

        If nombreReal <> "" Then
            ' Aquí llama al formulario que está en el otro archivo (panel.vb)
            Dim panelPrincipalUsuario As New frmPanelUsuario(nombreReal)
            panelPrincipalUsuario.Show()
            Me.Hide()
        Else
            MessageBox.Show("Credenciales incorrectas.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtPassword.Clear()
        End If
    End Sub

    Private Sub btnRegistrar_Click(sender As Object, e As EventArgs) Handles btnRegistrar.Click
        Dim ventanaRegistro As New frmRegistro()
        Me.Hide()
        ventanaRegistro.ShowDialog()
        Me.Show()
    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Application.Exit()
    End Sub
End Class

' =========================================================================================
' CLASE 2: REGISTRO DE USUARIO
' =========================================================================================
Public Class frmRegistro
    Inherits Form

    Private txtRegNombre As New TextBox()
    Private txtRegPassword As New TextBox()
    Private WithEvents btnCrearCuenta As New Button()
    Private WithEvents btnVolver As New Button()

    Public Sub New()
        Me.AutoScaleMode = AutoScaleMode.None
        Me.Text = "Regístrate"
        Me.Size = New Size(450, 600)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.FromArgb(18, 18, 18)
        Me.ForeColor = Color.White
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        ConfigurarRegistroUI()
    End Sub

    Private Sub ConfigurarRegistroUI()
        Dim lblTitulo As New Label() With {.Text = "Crea tu cuenta", .Bounds = New Rectangle(0, 50, 450, 40), .Font = New Font("Segoe UI Black", 24.0F, FontStyle.Bold), .TextAlign = ContentAlignment.MiddleCenter}
        CrearCampoUI("Nombre de usuario:", txtRegNombre, 150)
        CrearCampoUI("Contraseña:", txtRegPassword, 270)
        txtRegPassword.PasswordChar = "•"c

        btnCrearCuenta.Text = "CREAR CUENTA"
        btnCrearCuenta.Bounds = New Rectangle(65, 410, 315, 55)
        btnCrearCuenta.BackColor = Color.FromArgb(30, 215, 96)
        btnCrearCuenta.ForeColor = Color.Black
        btnCrearCuenta.FlatStyle = FlatStyle.Flat
        btnCrearCuenta.FlatAppearance.BorderSize = 0
        btnCrearCuenta.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        btnCrearCuenta.Cursor = Cursors.Hand

        btnVolver.Text = "Volver al inicio de sesión"
        btnVolver.Bounds = New Rectangle(65, 490, 315, 30)
        btnVolver.BackColor = Color.Transparent
        btnVolver.ForeColor = Color.LightGray
        btnVolver.FlatStyle = FlatStyle.Flat
        btnVolver.FlatAppearance.BorderSize = 0
        btnVolver.Font = New Font("Segoe UI", 10.0F, FontStyle.Underline)
        btnVolver.Cursor = Cursors.Hand

        Me.Controls.Add(lblTitulo) : Me.Controls.Add(btnCrearCuenta) : Me.Controls.Add(btnVolver)
    End Sub

    Private Sub CrearCampoUI(texto As String, caja As TextBox, y As Integer)
        Dim lbl As New Label() With {.Text = texto, .Bounds = New Rectangle(65, y, 300, 25), .Font = New Font("Segoe UI", 11.0F, FontStyle.Bold)}
        caja.AutoSize = False
        caja.Bounds = New Rectangle(65, y + 35, 315, 45)
        caja.Font = New Font("Segoe UI", 14.0F)
        caja.BackColor = Color.FromArgb(40, 40, 40)
        caja.ForeColor = Color.White
        caja.BorderStyle = BorderStyle.FixedSingle
        Me.Controls.Add(lbl) : Me.Controls.Add(caja)
    End Sub

    Private Sub btnCrearCuenta_Click(sender As Object, e As EventArgs) Handles btnCrearCuenta.Click
        Dim nombre = txtRegNombre.Text.Trim()
        Dim pass = txtRegPassword.Text.Trim()
        If nombre = "" Or pass = "" Then MessageBox.Show("Ingresa usuario y contraseña.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning) : Return

        Dim emailFalso As String = nombre.Replace(" ", "").ToLower() & New Random().Next(10000, 99999).ToString() & "@spoticloud.local"
        Try
            Using con = GetConnection()
                Using cmd As New SqlCommand("INSERT INTO Usuarios (Nombre, Email, Contrasena) VALUES (@Nombre, @Email, @Pass)", con)
                    cmd.Parameters.AddWithValue("@Nombre", nombre) : cmd.Parameters.AddWithValue("@Email", emailFalso) : cmd.Parameters.AddWithValue("@Pass", pass)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            MessageBox.Show("¡Cuenta creada! Usuario: " & nombre, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Error al crear cuenta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Me.Close()
    End Sub
End Class

' =========================================================================================
' CLASE 3: PANEL DE ADMINISTRACIÓN PRINCIPAL (ADMIN)
' =========================================================================================
Public Class frmAdmin
    Inherits Form

#Region "Controles del Admin"
    Private menuPrincipal As New MenuStrip()
    Private WithEvents btnCrearArtista As New Button()
    Private WithEvents btnActualizarGenero As New Button()
    Private WithEvents btnEliminarArtista As New Button()
    Private txtNombreArtista As New TextBox()
    Private txtGeneroArtista As New TextBox()
    Private txtIdArtista As New TextBox()
    Private txtNuevoGenero As New TextBox()
    Private WithEvents btnCrearCancion As New Button()
    Private WithEvents btnEliminarCancion As New Button()
    Private txtTituloCancion As New TextBox()
    Private txtIdArtistaCancion As New TextBox()
    Private txtDuracion As New TextBox()
    Private txtGeneroCancion As New TextBox()
    Private txtIdCancionAccion As New TextBox()
    Private dtpFechaLanzamiento As New DateTimePicker()
    Private WithEvents btnCrearPlaylist As New Button()
    Private WithEvents btnEliminarPlaylist As New Button()
    Private WithEvents btnVerCancionesPlaylist As New Button()
    Private WithEvents btnAgregarCancionPlaylist As New Button()
    Private WithEvents btnQuitarCancionPlaylist As New Button()
    Private txtNombrePlaylist As New TextBox()
    Private txtIdUsuarioPropietario As New TextBox()
    Private txtIdPlaylistAccion As New TextBox()
    Private txtIdCancionBridge As New TextBox()
    Private dgvArtistas As New DataGridView()
    Private dgvCanciones As New DataGridView()
    Private dgvPlaylists As New DataGridView()
#End Region

    Public Sub New()
        Me.AutoScaleMode = AutoScaleMode.None
        Me.Text = "SpotiCloud - Panel Principal de DBA"
        Me.Size = New Size(1150, 900)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.FromArgb(30, 30, 30)
        Me.ForeColor = Color.White
        Me.Font = New Font("Segoe UI", 9.5F)
        AddHandler Me.FormClosed, Sub() Application.Exit()

        ConfigurarMenuSuperior()
        ConfigurarInterfazUI()
        CargarDatosEnGrids()
    End Sub

    Private Sub ConfigurarMenuSuperior()
        menuPrincipal.BackColor = Color.FromArgb(40, 40, 40)
        menuPrincipal.ForeColor = Color.White
        menuPrincipal.Font = New Font("Segoe UI", 10.0F)

        Dim menuOrdenar As New ToolStripMenuItem("🔀 Ordenar Tablas")
        Dim itemArtAZ As New ToolStripMenuItem("Artistas (A - Z)", Nothing, Sub(s, e) OrdenarGrid(dgvArtistas, "Nombre ASC"))
        Dim itemArtZA As New ToolStripMenuItem("Artistas (Z - A)", Nothing, Sub(s, e) OrdenarGrid(dgvArtistas, "Nombre DESC"))
        Dim itemPlayNew As New ToolStripMenuItem("Playlists (Más Recientes)", Nothing, Sub(s, e) OrdenarGrid(dgvPlaylists, "Creada DESC"))
        Dim itemPlayOld As New ToolStripMenuItem("Playlists (Antiguas)", Nothing, Sub(s, e) OrdenarGrid(dgvPlaylists, "Creada ASC"))

        menuOrdenar.DropDownItems.AddRange(New ToolStripItem() {itemArtAZ, itemArtZA, New ToolStripSeparator(), itemPlayNew, itemPlayOld})
        menuPrincipal.Items.Add(menuOrdenar)
        Me.Controls.Add(menuPrincipal)
        Me.MainMenuStrip = menuPrincipal
    End Sub

    Private Sub OrdenarGrid(dgv As DataGridView, sortString As String)
        Dim dt As DataTable = TryCast(dgv.DataSource, DataTable)
        If dt IsNot Nothing Then dt.DefaultView.Sort = sortString
    End Sub

    Private Sub ConfigurarInterfazUI()
        Dim grpArtistas As New GroupBox() With {.Text = "Gestión de Artistas", .Bounds = New Rectangle(20, 40, 420, 230), .ForeColor = Color.MediumSpringGreen}
        CrearCampoUI(grpArtistas, "Nombre:", txtNombreArtista, 15, 30)
        CrearCampoUI(grpArtistas, "Género:", txtGeneroArtista, 15, 65)
        ConfigurarBotonUI(grpArtistas, btnCrearArtista, "Crear Artista", 260, 60, Color.SeaGreen)
        CrearCampoUI(grpArtistas, "ID Artista:", txtIdArtista, 15, 120)
        CrearCampoUI(grpArtistas, "Nuevo Género:", txtNuevoGenero, 15, 155)
        ConfigurarBotonUI(grpArtistas, btnActualizarGenero, "Actualizar Género", 260, 115, Color.SteelBlue)
        ConfigurarBotonUI(grpArtistas, btnEliminarArtista, "Eliminar Artista", 260, 155, Color.IndianRed)
        Me.Controls.Add(grpArtistas)
        ConfigurarGrid(dgvArtistas, 460, 50, 640, 220)

        Dim grpCanciones As New GroupBox() With {.Text = "Gestión de Canciones", .Bounds = New Rectangle(20, 290, 420, 270), .ForeColor = Color.MediumSpringGreen}
        CrearCampoUI(grpCanciones, "Título:", txtTituloCancion, 15, 30)
        CrearCampoUI(grpCanciones, "ID Artista:", txtIdArtistaCancion, 15, 65)
        CrearCampoUI(grpCanciones, "Duración (s):", txtDuracion, 15, 100)
        CrearCampoUI(grpCanciones, "Género:", txtGeneroCancion, 15, 135)
        Dim lblFecha As New Label() With {.Text = "Fecha Lanz.:", .Bounds = New Rectangle(15, 170, 100, 25), .ForeColor = Color.White}
        dtpFechaLanzamiento.Bounds = New Rectangle(115, 170, 130, 25)
        dtpFechaLanzamiento.Format = DateTimePickerFormat.Short
        grpCanciones.Controls.Add(lblFecha) : grpCanciones.Controls.Add(dtpFechaLanzamiento)
        ConfigurarBotonUI(grpCanciones, btnCrearCancion, "Registrar Canción", 260, 160, Color.SeaGreen)
        CrearCampoUI(grpCanciones, "ID Canción:", txtIdCancionAccion, 15, 225)
        ConfigurarBotonUI(grpCanciones, btnEliminarCancion, "Eliminar Canción", 260, 220, Color.IndianRed)
        Me.Controls.Add(grpCanciones)
        ConfigurarGrid(dgvCanciones, 460, 300, 640, 260)

        Dim grpPlaylists As New GroupBox() With {.Text = "Gestión de Playlists y sus Canciones", .Bounds = New Rectangle(20, 580, 420, 240), .ForeColor = Color.MediumSpringGreen}
        CrearCampoUI(grpPlaylists, "Nombre List:", txtNombrePlaylist, 15, 30)
        CrearCampoUI(grpPlaylists, "ID Usuario:", txtIdUsuarioPropietario, 15, 65)
        ConfigurarBotonUI(grpPlaylists, btnCrearPlaylist, "Crear Playlist", 260, 45, Color.SeaGreen)
        CrearCampoUI(grpPlaylists, "ID Playlist:", txtIdPlaylistAccion, 15, 115)
        ConfigurarBotonUI(grpPlaylists, btnEliminarPlaylist, "Borrar Playlist", 260, 100, Color.IndianRed)
        ConfigurarBotonUI(grpPlaylists, btnVerCancionesPlaylist, "Ver Canciones", 260, 135, Color.DarkOrange)
        CrearCampoUI(grpPlaylists, "ID Canción:", txtIdCancionBridge, 15, 160)
        btnAgregarCancionPlaylist.Bounds = New Rectangle(15, 195, 185, 30) : btnQuitarCancionPlaylist.Bounds = New Rectangle(215, 195, 185, 30)
        ConfigurarBotonEstilo(btnAgregarCancionPlaylist, "Agregar a Lista", Color.SteelBlue)
        ConfigurarBotonEstilo(btnQuitarCancionPlaylist, "Quitar de Lista", Color.IndianRed)
        grpPlaylists.Controls.Add(btnAgregarCancionPlaylist) : grpPlaylists.Controls.Add(btnQuitarCancionPlaylist)
        Me.Controls.Add(grpPlaylists)
        ConfigurarGrid(dgvPlaylists, 460, 590, 640, 230)
    End Sub

    Private Sub CrearCampoUI(parent As GroupBox, lblText As String, txt As TextBox, x As Integer, y As Integer)
        Dim lbl As New Label() With {.Text = lblText, .Bounds = New Rectangle(x, y, 100, 25), .ForeColor = Color.White}
        txt.Bounds = New Rectangle(x + 100, y, 130, 25)
        parent.Controls.Add(lbl) : parent.Controls.Add(txt)
    End Sub

    Private Sub ConfigurarBotonUI(parent As GroupBox, btn As Button, text As String, x As Integer, y As Integer, bgCol As Color)
        btn.Bounds = New Rectangle(x, y, 140, 32)
        ConfigurarBotonEstilo(btn, text, bgCol)
        parent.Controls.Add(btn)
    End Sub

    Private Sub ConfigurarBotonEstilo(btn As Button, text As String, bgCol As Color)
        btn.Text = text : btn.BackColor = bgCol : btn.ForeColor = Color.White
        btn.FlatStyle = FlatStyle.Flat : btn.FlatAppearance.BorderSize = 0 : btn.Cursor = Cursors.Hand
    End Sub

    Private Sub ConfigurarGrid(dgv As DataGridView, x As Integer, y As Integer, w As Integer, h As Integer)
        dgv.Bounds = New Rectangle(x, y, w, h)
        dgv.BackgroundColor = Color.FromArgb(45, 45, 45) : dgv.ForeColor = Color.Black
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill : dgv.ReadOnly = True
        dgv.AllowUserToAddRows = False : dgv.RowHeadersVisible = False : dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        Me.Controls.Add(dgv)
    End Sub

    Private Sub CargarDatosEnGrids()
        Try
            Using con = GetConnection()
                Dim daArtistas As New SqlDataAdapter("SELECT IdArtista AS ID, Nombre, Genero FROM Artistas", con)
                Dim dtArtistas As New DataTable() : daArtistas.Fill(dtArtistas) : dgvArtistas.DataSource = dtArtistas

                Dim daCanciones As New SqlDataAdapter("SELECT IdCancion AS ID, Titulo, Genero, IdArtista AS [ID Artista], DuracionSegundos AS [Dur.(s)], FechaLanzamiento AS Lanzamiento FROM Canciones", con)
                Dim dtCanciones As New DataTable() : daCanciones.Fill(dtCanciones) : dgvCanciones.DataSource = dtCanciones

                Dim daPlaylists As New SqlDataAdapter("SELECT IdPlaylist AS ID, Nombre AS Playlist, IdUsuario AS [ID Creador], FechaCreacion AS Creada FROM Playlists", con)
                Dim dtPlaylists As New DataTable() : daPlaylists.Fill(dtPlaylists) : dgvPlaylists.DataSource = dtPlaylists
            End Using
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnCrearArtista_Click(sender As Object, e As EventArgs) Handles btnCrearArtista.Click
        Try
            Using con = GetConnection(), cmd As New SqlCommand("sp_InsertarArtista", con)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Nombre", txtNombreArtista.Text.Trim()) : cmd.Parameters.AddWithValue("@Genero", txtGeneroArtista.Text.Trim())
                cmd.ExecuteNonQuery()
            End Using
            CargarDatosEnGrids() : txtNombreArtista.Clear() : txtGeneroArtista.Clear()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnActualizarGenero_Click(sender As Object, e As EventArgs) Handles btnActualizarGenero.Click
        Try
            Using con = GetConnection(), cmd As New SqlCommand("sp_ActualizarGeneroArtista", con)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@IdArtista", Convert.ToInt32(txtIdArtista.Text)) : cmd.Parameters.AddWithValue("@NuevoGenero", txtNuevoGenero.Text.Trim())
                cmd.ExecuteNonQuery()
            End Using
            CargarDatosEnGrids() : txtIdArtista.Clear() : txtNuevoGenero.Clear()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnEliminarArtista_Click(sender As Object, e As EventArgs) Handles btnEliminarArtista.Click
        Try
            Using con = GetConnection(), cmd As New SqlCommand("sp_EliminarArtista", con)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@IdArtista", Convert.ToInt32(txtIdArtista.Text))
                cmd.ExecuteNonQuery()
            End Using
            CargarDatosEnGrids() : txtIdArtista.Clear()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnCrearCancion_Click(sender As Object, e As EventArgs) Handles btnCrearCancion.Click
        Try
            Using con = GetConnection(), cmd As New SqlCommand("sp_InsertarCancion", con)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Titulo", txtTituloCancion.Text.Trim())
                cmd.Parameters.AddWithValue("@IdArtista", Convert.ToInt32(txtIdArtistaCancion.Text))
                cmd.Parameters.AddWithValue("@DuracionSegundos", Convert.ToInt32(txtDuracion.Text))
                cmd.Parameters.AddWithValue("@Genero", txtGeneroCancion.Text.Trim())
                cmd.Parameters.AddWithValue("@FechaLanzamiento", dtpFechaLanzamiento.Value.Date)
                cmd.ExecuteNonQuery()
            End Using
            CargarDatosEnGrids()
            txtTituloCancion.Clear() : txtIdArtistaCancion.Clear() : txtDuracion.Clear() : txtGeneroCancion.Clear()
        Catch ex As Exception
            MessageBox.Show("Error al registrar canción. Verifique el ID de artista.", "Error")
        End Try
    End Sub

    Private Sub btnEliminarCancion_Click(sender As Object, e As EventArgs) Handles btnEliminarCancion.Click
        Try
            Using con = GetConnection(), cmd As New SqlCommand("sp_EliminarCancion", con)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@IdCancion", Convert.ToInt32(txtIdCancionAccion.Text))
                cmd.ExecuteNonQuery()
            End Using
            CargarDatosEnGrids() : txtIdCancionAccion.Clear()
            MessageBox.Show("Canción eliminada correctamente.")
        Catch ex As Exception
            MessageBox.Show("Error al eliminar la canción. Verifique el ID.")
        End Try
    End Sub

    Private Sub btnCrearPlaylist_Click(sender As Object, e As EventArgs) Handles btnCrearPlaylist.Click
        Try
            Using con = GetConnection(), cmd As New SqlCommand("sp_InsertarPlaylist", con)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Nombre", txtNombrePlaylist.Text.Trim()) : cmd.Parameters.AddWithValue("@IdUsuario", Convert.ToInt32(txtIdUsuarioPropietario.Text))
                cmd.ExecuteNonQuery()
            End Using
            CargarDatosEnGrids() : txtNombrePlaylist.Clear() : txtIdUsuarioPropietario.Clear()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnEliminarPlaylist_Click(sender As Object, e As EventArgs) Handles btnEliminarPlaylist.Click
        Try
            Using con = GetConnection(), cmd As New SqlCommand("sp_EliminarPlaylist", con)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@IdPlaylist", Convert.ToInt32(txtIdPlaylistAccion.Text))
                cmd.ExecuteNonQuery()
            End Using
            CargarDatosEnGrids() : txtIdPlaylistAccion.Clear()
            MessageBox.Show("Playlist eliminada.")
        Catch ex As Exception
            MessageBox.Show("Error al eliminar Playlist.")
        End Try
    End Sub

    Private Sub btnVerCancionesPlaylist_Click(sender As Object, e As EventArgs) Handles btnVerCancionesPlaylist.Click
        Dim idPlay As String = txtIdPlaylistAccion.Text.Trim()
        If idPlay = "" Then MessageBox.Show("Ingresa el ID.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information) : Return

        Dim formCanciones As New Form With {.Text = "Canciones de Playlist ID: " & idPlay, .Size = New Size(550, 350), .StartPosition = FormStartPosition.CenterParent, .BackColor = Color.FromArgb(40, 40, 40)}
        Dim dgvDetalles As New DataGridView With {.Dock = DockStyle.Fill, .ReadOnly = True, .AllowUserToAddRows = False, .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, .RowHeadersVisible = False, .SelectionMode = DataGridViewSelectionMode.FullRowSelect}
        formCanciones.Controls.Add(dgvDetalles)

        Try
            Using con = GetConnection()
                Dim query As String = "SELECT c.IdCancion AS ID, c.Titulo AS Canción, c.Genero AS Género, a.Nombre AS Artista, c.DuracionSegundos AS [Duración(s)] FROM Canciones c INNER JOIN PlaylistCanciones pc ON c.IdCancion = pc.IdCancion INNER JOIN Artistas a ON c.IdArtista = a.IdArtista WHERE pc.IdPlaylist = @Id"
                Using cmd As New SqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(idPlay))
                    Dim da As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    dgvDetalles.DataSource = dt
                End Using
            End Using
            formCanciones.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("Error al cargar canciones: " & ex.Message)
        End Try
    End Sub

    Private Sub btnAgregarCancionPlaylist_Click(sender As Object, e As EventArgs) Handles btnAgregarCancionPlaylist.Click
        Try
            Using con = GetConnection(), cmd As New SqlCommand("sp_AgregarCancionPlaylist", con)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@IdPlaylist", Convert.ToInt32(txtIdPlaylistAccion.Text)) : cmd.Parameters.AddWithValue("@IdCancion", Convert.ToInt32(txtIdCancionBridge.Text))
                cmd.ExecuteNonQuery()
            End Using
            MessageBox.Show("Canción enlazada correctamente.")
            txtIdCancionBridge.Clear()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnQuitarCancionPlaylist_Click(sender As Object, e As EventArgs) Handles btnQuitarCancionPlaylist.Click
        Try
            Using con = GetConnection(), cmd As New SqlCommand("sp_QuitarCancionPlaylist", con)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@IdPlaylist", Convert.ToInt32(txtIdPlaylistAccion.Text)) : cmd.Parameters.AddWithValue("@IdCancion", Convert.ToInt32(txtIdCancionBridge.Text))
                cmd.ExecuteNonQuery()
            End Using
            MessageBox.Show("Canción removida de la lista.")
            txtIdCancionBridge.Clear()
        Catch ex As Exception
        End Try
    End Sub
End Class