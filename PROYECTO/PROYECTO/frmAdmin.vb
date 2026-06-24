Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

' =========================================================================================
' FORMULARIO: PANEL PRINCIPAL DEL USUARIO
' SPA con Live Search moderno, Wildcards ocultos y Sistema de Favoritos (Likes)
' =========================================================================================
Public Class frmPanelUsuario
    Inherits Form

    Private usuarioActualId As Integer = 0 ' ID del usuario para guardar sus Favoritos

    ' Paneles Estructurales
    Private pnlReproductorSuperior As New System.Windows.Forms.Panel()
    Private pnlNavegacionInferior As New System.Windows.Forms.Panel()
    Private pnlPrincipal As New System.Windows.Forms.Panel()

    ' Sub-paneles de Vistas
    Private pnlVistaGeneros As New System.Windows.Forms.Panel()
    Private pnlVistaCanciones As New System.Windows.Forms.Panel()
    Private pnlVistaBuscar As New System.Windows.Forms.Panel()
    Private pnlVistaFavoritos As New System.Windows.Forms.Panel()

    ' Controles de Datos
    Private dgvCancionesUsuario As New DataGridView()
    Private dgvResultadosBusqueda As New DataGridView()
    Private dgvFavoritos As New DataGridView()
    Private lblTituloGenero As New Label()

    ' Controles Búsqueda Moderna
    Private cmbCriterioBusqueda As New ComboBox()
    Private WithEvents txtBusqueda As New TextBox()

    Public Sub New(nombreUsuario As String)
        Me.AutoScaleMode = AutoScaleMode.None
        Me.Text = "SpotiCloud Web Player"
        Me.Size = New Size(1280, 850)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.FromArgb(18, 18, 18)
        Me.ForeColor = Color.White
        Me.Font = New Font("Segoe UI", 10.0F)
        AddHandler Me.FormClosed, Sub() Application.Exit()

        ' Obtener el ID del Usuario silenciosamente
        ObtenerIdUsuario(nombreUsuario)

        ConfigurarReproductorArriba()
        ConfigurarNavegacionAbajo()
        ConfigurarAreaPrincipal(nombreUsuario)
    End Sub

    Private Sub ObtenerIdUsuario(nombreUsuario As String)
        Try
            Using con = DatabaseConnection.GetConnection()
                Dim cmd As New SqlCommand("SELECT IdUsuario FROM Usuarios WHERE Nombre = @Nombre", con)
                cmd.Parameters.AddWithValue("@Nombre", nombreUsuario)
                Dim res = cmd.ExecuteScalar()
                If res IsNot Nothing Then usuarioActualId = Convert.ToInt32(res)
            End Using
        Catch ex As Exception
        End Try
    End Sub

    ' =========================================================================
    ' ESTRUCTURA: REPRODUCTOR Y NAVEGACIÓN
    ' =========================================================================
    Private Sub ConfigurarReproductorArriba()
        pnlReproductorSuperior.BackColor = Color.FromArgb(24, 24, 24)
        pnlReproductorSuperior.Height = 90
        pnlReproductorSuperior.Dock = DockStyle.Top
        Dim bordeBottom As New System.Windows.Forms.Panel() With {.BackColor = Color.FromArgb(40, 40, 40), .Dock = DockStyle.Bottom, .Height = 1}
        pnlReproductorSuperior.Controls.Add(bordeBottom)
        Me.Controls.Add(pnlReproductorSuperior)
    End Sub

    Private Sub ConfigurarNavegacionAbajo()
        pnlNavegacionInferior.BackColor = Color.FromArgb(0, 0, 0)
        pnlNavegacionInferior.Height = 80
        pnlNavegacionInferior.Dock = DockStyle.Bottom
        Me.Controls.Add(pnlNavegacionInferior)

        Dim anchoBoton As Integer = 1280 / 4

        Dim btnInicio = CrearBotonNav("🏠 Inicio", 0, anchoBoton, True)
        Dim btnBuscar = CrearBotonNav("🔍 Buscar", anchoBoton, anchoBoton, False)
        Dim btnCrearPlaylist = CrearBotonNav("➕ Crear Playlist", anchoBoton * 2, anchoBoton, False)
        Dim btnFavoritos = CrearBotonNav("❤️ Favoritos", anchoBoton * 3, anchoBoton, False)

        AddHandler btnInicio.Click, Sub(s, e) MostrarVista(pnlVistaGeneros, btnInicio)
        AddHandler btnBuscar.Click, Sub(s, e) MostrarVista(pnlVistaBuscar, btnBuscar)
        AddHandler btnFavoritos.Click, Sub(s, e)
                                           CargarFavoritos()
                                           MostrarVista(pnlVistaFavoritos, btnFavoritos)
                                       End Sub

        pnlNavegacionInferior.Controls.Add(btnInicio)
        pnlNavegacionInferior.Controls.Add(btnBuscar)
        pnlNavegacionInferior.Controls.Add(btnCrearPlaylist)
        pnlNavegacionInferior.Controls.Add(btnFavoritos)
    End Sub

    Private Function CrearBotonNav(texto As String, x As Integer, w As Integer, activo As Boolean) As Button
        Dim btn As New Button() With {.Text = texto, .Bounds = New Rectangle(x, 0, w, 80), .FlatStyle = FlatStyle.Flat, .BackColor = If(activo, Color.FromArgb(30, 30, 30), Color.Transparent), .ForeColor = If(activo, Color.White, Color.FromArgb(160, 160, 160)), .Font = New Font("Segoe UI", 12.0F, FontStyle.Bold), .Cursor = Cursors.Hand}
        btn.FlatAppearance.BorderSize = 0 : btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 40, 40)
        Return btn
    End Function

    Private Sub MostrarVista(panelAMostrar As System.Windows.Forms.Panel, botonActivo As Button)
        pnlVistaGeneros.Visible = False
        pnlVistaCanciones.Visible = False
        pnlVistaBuscar.Visible = False
        pnlVistaFavoritos.Visible = False

        panelAMostrar.Visible = True

        For Each ctrl As Control In pnlNavegacionInferior.Controls
            If TypeOf ctrl Is Button Then
                Dim btn = DirectCast(ctrl, Button)
                btn.BackColor = If(btn Is botonActivo, Color.FromArgb(30, 30, 30), Color.Transparent)
                btn.ForeColor = If(btn Is botonActivo, Color.White, Color.FromArgb(160, 160, 160))
            End If
        Next
    End Sub

    ' =========================================================================
    ' ÁREA PRINCIPAL
    ' =========================================================================
    Private Sub ConfigurarAreaPrincipal(nombreUsuario As String)
        pnlPrincipal.BackColor = Color.FromArgb(18, 18, 18)
        pnlPrincipal.Dock = DockStyle.Fill
        Me.Controls.Add(pnlPrincipal)

        ConfigurarPantallaGeneros(nombreUsuario)
        ConfigurarPantallaCanciones()
        ConfigurarPantallaBusqueda()
        ConfigurarPantallaFavoritos()
    End Sub

    ' ---------------------------------------------------------
    ' PANTALLA 1: INICIO (GÉNEROS)
    ' ---------------------------------------------------------
    Private Sub ConfigurarPantallaGeneros(nombreUsuario As String)
        pnlVistaGeneros.Dock = DockStyle.Fill : pnlVistaGeneros.AutoScroll = True : pnlVistaGeneros.Visible = True
        pnlPrincipal.Controls.Add(pnlVistaGeneros)

        Dim lblBienvenida As New Label() With {.Text = "Bienvenido " & nombreUsuario, .Bounds = New Rectangle(60, 30, 800, 50), .Font = New Font("Segoe UI Black", 26.0F, FontStyle.Bold), .ForeColor = Color.White, .TextAlign = ContentAlignment.BottomLeft}
        Dim lblSeccionGen As New Label() With {.Text = "Explorar Géneros", .Bounds = New Rectangle(60, 95, 400, 35), .Font = New Font("Segoe UI", 14.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(179, 179, 179), .TextAlign = ContentAlignment.MiddleLeft}

        pnlVistaGeneros.Controls.Add(lblBienvenida) : pnlVistaGeneros.Controls.Add(lblSeccionGen)
        CargarGenerosDesdeBaseDeDatos()
    End Sub

    ' ---------------------------------------------------------
    ' PANTALLA 2: CANCIONES POR GÉNERO
    ' ---------------------------------------------------------
    Private Sub ConfigurarPantallaCanciones()
        pnlVistaCanciones.Dock = DockStyle.Fill : pnlVistaCanciones.Visible = False
        pnlPrincipal.Controls.Add(pnlVistaCanciones)

        Dim btnVolver As New Button() With {.Text = "⬅ Volver", .Bounds = New Rectangle(60, 20, 120, 40), .FlatStyle = FlatStyle.Flat, .BackColor = Color.FromArgb(45, 45, 45), .ForeColor = Color.White, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .Cursor = Cursors.Hand}
        btnVolver.FlatAppearance.BorderSize = 0
        AddHandler btnVolver.Click, Sub() MostrarVista(pnlVistaGeneros, pnlNavegacionInferior.Controls(0))

        ' BOTÓN PARA DAR LIKE
        Dim btnLike As New Button() With {.Text = "❤️ Dar Like", .Bounds = New Rectangle(200, 20, 150, 40), .FlatStyle = FlatStyle.Flat, .BackColor = Color.FromArgb(233, 64, 87), .ForeColor = Color.White, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .Cursor = Cursors.Hand}
        btnLike.FlatAppearance.BorderSize = 0
        AddHandler btnLike.Click, Sub() DarLikeACancion(dgvCancionesUsuario)

        lblTituloGenero.Bounds = New Rectangle(60, 85, 1000, 50)
        lblTituloGenero.Font = New Font("Segoe UI Black", 24.0F, FontStyle.Bold)
        lblTituloGenero.ForeColor = Color.FromArgb(30, 215, 96)

        ConfigurarEstiloGrid(dgvCancionesUsuario)
        dgvCancionesUsuario.Bounds = New Rectangle(60, 150, 1150, 470)

        pnlVistaCanciones.Controls.Add(btnVolver)
        pnlVistaCanciones.Controls.Add(btnLike)
        pnlVistaCanciones.Controls.Add(lblTituloGenero)
        pnlVistaCanciones.Controls.Add(dgvCancionesUsuario)
    End Sub

    ' ---------------------------------------------------------
    ' PANTALLA 3: BÚSQUEDA MODERNA (LIVE SEARCH + WILDCARDS)
    ' ---------------------------------------------------------
    Private Sub ConfigurarPantallaBusqueda()
        pnlVistaBuscar.Dock = DockStyle.Fill : pnlVistaBuscar.Visible = False
        pnlPrincipal.Controls.Add(pnlVistaBuscar)

        Dim lblTituloBusqueda As New Label() With {.Text = "Búsqueda Musical", .Bounds = New Rectangle(60, 30, 800, 50), .Font = New Font("Segoe UI Black", 26.0F, FontStyle.Bold), .ForeColor = Color.White, .TextAlign = ContentAlignment.BottomLeft}

        ' ICONO LUPA (En lugar del botón Buscar)
        Dim lblLupa As New Label() With {.Text = "🔍", .Bounds = New Rectangle(60, 105, 40, 40), .Font = New Font("Segoe UI", 18.0F), .ForeColor = Color.White, .TextAlign = ContentAlignment.MiddleCenter}

        ' COMBOBOX
        cmbCriterioBusqueda.Bounds = New Rectangle(110, 105, 180, 35)
        cmbCriterioBusqueda.Font = New Font("Segoe UI", 14.0F)
        cmbCriterioBusqueda.DropDownStyle = ComboBoxStyle.DropDownList
        cmbCriterioBusqueda.Items.Add("Artista") : cmbCriterioBusqueda.Items.Add("Género") : cmbCriterioBusqueda.Items.Add("Canción")
        cmbCriterioBusqueda.SelectedIndex = 0
        cmbCriterioBusqueda.BackColor = Color.FromArgb(40, 40, 40)
        cmbCriterioBusqueda.ForeColor = Color.White
        cmbCriterioBusqueda.FlatStyle = FlatStyle.Flat

        ' TEXTBOX ESTILO MODERNO
        txtBusqueda.AutoSize = False
        txtBusqueda.Bounds = New Rectangle(310, 105, 450, 35)
        txtBusqueda.Font = New Font("Segoe UI", 14.0F)
        txtBusqueda.BackColor = Color.FromArgb(18, 18, 18) ' Se funde con el fondo
        txtBusqueda.ForeColor = Color.White
        txtBusqueda.BorderStyle = BorderStyle.None ' Sin bordes

        ' LÍNEA INFERIOR PARA TEXTBOX
        Dim pnlLinea As New System.Windows.Forms.Panel() With {.Bounds = New Rectangle(310, 140, 450, 2), .BackColor = Color.FromArgb(30, 215, 96)}

        ' BOTÓN PARA DAR LIKE DESDE BÚSQUEDA
        Dim btnLike As New Button() With {.Text = "❤️ Dar Like", .Bounds = New Rectangle(790, 100, 150, 40), .FlatStyle = FlatStyle.Flat, .BackColor = Color.FromArgb(233, 64, 87), .ForeColor = Color.White, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .Cursor = Cursors.Hand}
        btnLike.FlatAppearance.BorderSize = 0
        AddHandler btnLike.Click, Sub() DarLikeACancion(dgvResultadosBusqueda)

        ConfigurarEstiloGrid(dgvResultadosBusqueda)
        dgvResultadosBusqueda.Bounds = New Rectangle(60, 170, 1150, 450)

        pnlVistaBuscar.Controls.Add(lblTituloBusqueda)
        pnlVistaBuscar.Controls.Add(lblLupa)
        pnlVistaBuscar.Controls.Add(cmbCriterioBusqueda)
        pnlVistaBuscar.Controls.Add(txtBusqueda)
        pnlVistaBuscar.Controls.Add(pnlLinea)
        pnlVistaBuscar.Controls.Add(btnLike)
        pnlVistaBuscar.Controls.Add(dgvResultadosBusqueda)
    End Sub

    ' EVENTO: LIVE SEARCH (Se activa automático mientras el usuario escribe)
    Private Sub txtBusqueda_TextChanged(sender As Object, e As EventArgs) Handles txtBusqueda.TextChanged
        Dim termino As String = txtBusqueda.Text.Trim()
        Dim criterio As String = cmbCriterioBusqueda.SelectedItem.ToString()

        If termino = "" Then
            dgvResultadosBusqueda.DataSource = Nothing
            Return
        End If

        Try
            Using con = DatabaseConnection.GetConnection()
                Dim query As String = "SELECT c.IdCancion AS ID, c.Titulo AS [Canción], a.Nombre AS [Artista], c.Genero AS [Género], CAST(c.DuracionSegundos / 60 AS VARCHAR) + ':' + RIGHT('0' + CAST(c.DuracionSegundos % 60 AS VARCHAR), 2) AS [Duración] FROM Canciones c INNER JOIN Artistas a ON c.IdArtista = a.IdArtista "

                ' Filtro SQL Dinámico
                If criterio = "Artista" Then
                    query &= "WHERE a.Nombre LIKE @Filtro ORDER BY c.Titulo ASC"
                ElseIf criterio = "Género" Then
                    query &= "WHERE c.Genero LIKE @Filtro ORDER BY c.Titulo ASC"
                Else
                    query &= "WHERE c.Titulo LIKE @Filtro ORDER BY c.Titulo ASC"
                End If

                Using cmd As New SqlCommand(query, con)
                    ' EL WILDCARD MÁGICO (%): Busca lo que sea antes y después de tu palabra
                    cmd.Parameters.AddWithValue("@Filtro", "%" & termino & "%")

                    Dim da As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    dgvResultadosBusqueda.DataSource = dt

                    ' Ocultamos el ID para que el usuario no lo vea, pero lo podamos usar para el Like
                    If dgvResultadosBusqueda.Columns.Contains("ID") Then dgvResultadosBusqueda.Columns("ID").Visible = False
                End Using
            End Using
        Catch ex As Exception
        End Try
    End Sub

    ' ---------------------------------------------------------
    ' PANTALLA 4: TUS FAVORITOS (LIKES)
    ' ---------------------------------------------------------
    Private Sub ConfigurarPantallaFavoritos()
        pnlVistaFavoritos.Dock = DockStyle.Fill : pnlVistaFavoritos.Visible = False
        pnlPrincipal.Controls.Add(pnlVistaFavoritos)

        Dim lblTitulo As New Label() With {.Text = "Tus Canciones Favoritas ❤️", .Bounds = New Rectangle(60, 30, 800, 50), .Font = New Font("Segoe UI Black", 26.0F, FontStyle.Bold), .ForeColor = Color.White, .TextAlign = ContentAlignment.BottomLeft}

        Dim btnDislike As New Button() With {.Text = "💔 Quitar Like", .Bounds = New Rectangle(60, 100, 150, 40), .FlatStyle = FlatStyle.Flat, .BackColor = Color.FromArgb(45, 45, 45), .ForeColor = Color.White, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .Cursor = Cursors.Hand}
        btnDislike.FlatAppearance.BorderSize = 0
        AddHandler btnDislike.Click, Sub()
                                         QuitarLikeACancion()
                                         CargarFavoritos() ' Refresca la tabla
                                     End Sub

        ConfigurarEstiloGrid(dgvFavoritos)
        dgvFavoritos.Bounds = New Rectangle(60, 160, 1150, 460)

        pnlVistaFavoritos.Controls.Add(lblTitulo)
        pnlVistaFavoritos.Controls.Add(btnDislike)
        pnlVistaFavoritos.Controls.Add(dgvFavoritos)
    End Sub

    Private Sub CargarFavoritos()
        If usuarioActualId = 0 Then Return
        Try
            Using con = DatabaseConnection.GetConnection()
                Dim query As String = "SELECT c.IdCancion AS ID, c.Titulo AS [Canción], a.Nombre AS [Artista], c.Genero AS [Género], CAST(c.DuracionSegundos / 60 AS VARCHAR) + ':' + RIGHT('0' + CAST(c.DuracionSegundos % 60 AS VARCHAR), 2) AS [Duración] FROM Favoritos f INNER JOIN Canciones c ON f.IdCancion = c.IdCancion INNER JOIN Artistas a ON c.IdArtista = a.IdArtista WHERE f.IdUsuario = @IdUsuario ORDER BY f.FechaAgregado DESC"
                Using cmd As New SqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@IdUsuario", usuarioActualId)
                    Dim da As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    dgvFavoritos.DataSource = dt
                    If dgvFavoritos.Columns.Contains("ID") Then dgvFavoritos.Columns("ID").Visible = False
                End Using
            End Using
        Catch ex As Exception
        End Try
    End Sub

    ' =========================================================================
    ' LÓGICA DEL SISTEMA DE LIKES
    ' =========================================================================
    Private Sub DarLikeACancion(grid As DataGridView)
        If usuarioActualId = 0 Then Return
        If grid.SelectedRows.Count > 0 Then
            Dim idCancion As Integer = Convert.ToInt32(grid.SelectedRows(0).Cells("ID").Value)
            Dim nombreCancion As String = grid.SelectedRows(0).Cells("Canción").Value.ToString()

            Try
                Using con = DatabaseConnection.GetConnection()
                    Using cmd As New SqlCommand("sp_AgregarFavorito", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@IdUsuario", usuarioActualId)
                        cmd.Parameters.AddWithValue("@IdCancion", idCancion)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using
                MessageBox.Show("Agregaste '" & nombreCancion & "' a tus Favoritos ❤️", "Like", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("Hubo un error al guardar tu Like.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("Selecciona una canción primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub QuitarLikeACancion()
        If usuarioActualId = 0 OrElse dgvFavoritos.SelectedRows.Count = 0 Then Return
        Dim idCancion As Integer = Convert.ToInt32(dgvFavoritos.SelectedRows(0).Cells("ID").Value)
        Try
            Using con = DatabaseConnection.GetConnection()
                Using cmd As New SqlCommand("sp_QuitarFavorito", con)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@IdUsuario", usuarioActualId)
                    cmd.Parameters.AddWithValue("@IdCancion", idCancion)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
        End Try
    End Sub

    ' =========================================================================
    ' UTILIDADES VISUALES
    ' =========================================================================
    Private Sub ConfigurarEstiloGrid(dgv As DataGridView)
        dgv.BackgroundColor = Color.FromArgb(24, 24, 24)
        dgv.ForeColor = Color.Black
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgv.ReadOnly = True
        dgv.AllowUserToAddRows = False
        dgv.RowHeadersVisible = False
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
    End Sub

    Private Sub CargarGenerosDesdeBaseDeDatos()
        Dim paleta(,) As Color = {
            {Color.FromArgb(233, 64, 87), Color.FromArgb(242, 113, 33)},
            {Color.FromArgb(203, 45, 62), Color.FromArgb(115, 11, 23)},
            {Color.FromArgb(38, 208, 206), Color.FromArgb(26, 41, 128)},
            {Color.FromArgb(242, 153, 74), Color.FromArgb(242, 201, 76)},
            {Color.FromArgb(17, 153, 142), Color.FromArgb(56, 239, 125)},
            {Color.FromArgb(138, 35, 135), Color.FromArgb(233, 64, 87)},
            {Color.FromArgb(15, 32, 39), Color.FromArgb(44, 83, 100)},
            {Color.FromArgb(255, 175, 123), Color.FromArgb(215, 111, 48)}
        }

        Try
            Dim dtGeneros As New DataTable()
            Using con = DatabaseConnection.GetConnection()
                Dim da As New SqlDataAdapter("SELECT DISTINCT Genero FROM Canciones WHERE Genero IS NOT NULL ORDER BY Genero ASC", con)
                da.Fill(dtGeneros)
            End Using

            Dim mLeft As Integer = 60, mTop As Integer = 150
            Dim wCard As Integer = 260, hCard As Integer = 140
            Dim espX As Integer = 25, espY As Integer = 25
            Dim columna As Integer = 0, fila As Integer = 0, colorIndex As Integer = 0

            For Each row As DataRow In dtGeneros.Rows
                Dim genero As String = row("Genero").ToString()
                Dim posX = mLeft + (wCard + espX) * columna
                Dim posY = mTop + (hCard + espY) * fila
                Dim color1 = paleta(colorIndex Mod 8, 0)
                Dim color2 = paleta(colorIndex Mod 8, 1)

                Dim tarjeta = CrearTarjetaGenero(genero, posX, posY, wCard, hCard, color1, color2)
                pnlVistaGeneros.Controls.Add(tarjeta)

                columna += 1
                If columna > 3 Then
                    columna = 0 : fila += 1
                End If
                colorIndex += 1
            Next
        Catch ex As Exception
        End Try
    End Sub

    Private Function CrearTarjetaGenero(titulo As String, x As Integer, y As Integer, w As Integer, h As Integer, c1 As Color, c2 As Color) As System.Windows.Forms.Panel
        Dim tarjeta As New System.Windows.Forms.Panel() With {.Bounds = New Rectangle(x, y, w, h), .Cursor = Cursors.Hand}
        AddHandler tarjeta.Paint, Sub(sender As Object, e As PaintEventArgs)
                                      e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
                                      Using pincel As New LinearGradientBrush(New Rectangle(0, 0, w, h), c1, c2, LinearGradientMode.ForwardDiagonal)
                                          e.Graphics.FillRectangle(pincel, New Rectangle(0, 0, w, h))
                                      End Using
                                  End Sub

        Dim lbl As New Label() With {.Text = titulo, .Font = New Font("Segoe UI Black", 13.5F, FontStyle.Bold), .ForeColor = Color.White, .BackColor = Color.Transparent, .AutoSize = False, .Bounds = New Rectangle(10, 10, w - 20, h - 20), .TextAlign = ContentAlignment.MiddleCenter, .Cursor = Cursors.Hand}
        AddHandler tarjeta.Click, Sub() AbrirListaDeCanciones(titulo)
        AddHandler lbl.Click, Sub() AbrirListaDeCanciones(titulo)
        tarjeta.Controls.Add(lbl)
        Return tarjeta
    End Function

    Private Sub AbrirListaDeCanciones(genero As String)
        lblTituloGenero.Text = "Género: " & genero
        MostrarVista(pnlVistaCanciones, pnlNavegacionInferior.Controls(0)) ' Emula estar en Inicio

        Try
            Using con = DatabaseConnection.GetConnection()
                Dim query As String = "SELECT c.IdCancion AS ID, c.Titulo AS [Canción], a.Nombre AS [Artista], CAST(c.DuracionSegundos / 60 AS VARCHAR) + ':' + RIGHT('0' + CAST(c.DuracionSegundos % 60 AS VARCHAR), 2) AS [Duración], c.FechaLanzamiento AS [Lanzamiento] FROM Canciones c INNER JOIN Artistas a ON c.IdArtista = a.IdArtista WHERE c.Genero = @Genero ORDER BY c.Titulo ASC"
                Using cmd As New SqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@Genero", genero)
                    Dim da As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    dgvCancionesUsuario.DataSource = dt
                    If dgvCancionesUsuario.Columns.Contains("ID") Then dgvCancionesUsuario.Columns("ID").Visible = False
                End Using
            End Using
        Catch ex As Exception
        End Try
    End Sub

End Class