using System;
using System.Collections.Generic;
using EntreEspeciesNuevo.models;
using Microsoft.EntityFrameworkCore;

namespace EntreEspeciesNuevo.Models;

public partial class EntreespeciessqlContext : DbContext
{
    public EntreespeciessqlContext()
    {
    }

    public EntreespeciessqlContext(DbContextOptions<EntreespeciessqlContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<CitaExterna> CitaExternas { get; set; }

    public virtual DbSet<CitaInterna> CitaInternas { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<Configuracion> Configuracions { get; set; }

    public virtual DbSet<DetaCompra> DetaCompras { get; set; }
    public virtual DbSet<DetaVentum> DetaVenta { get; set; }

    public virtual DbSet<Mascota> Mascotas { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-HLHKBLE\\SQLEXPRESS;Initial Catalog=entreespeciessql;integrated security=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__categori__CB90334926310FE0");

            entity.ToTable("categorias");

            entity.Property(e => e.IdCategoria).HasColumnName("Id_Categoria");
            entity.Property(e => e.NomCategoria)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Nom_Categoria");
        });

        modelBuilder.Entity<CitaExterna>(entity =>
        {
            entity.HasKey(e => e.IdCitaExt).HasName("PK__cita_ext__4885B7F572037CA2");

            entity.ToTable("cita_externas");

            entity.Property(e => e.IdCitaExt).HasColumnName("Id_Cita_Ext");
            entity.Property(e => e.DocumentoCliente).HasColumnName("Documento_Cliente");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FechaHora)
                .HasColumnType("date")
                .HasColumnName("Fecha_Hora");
            entity.Property(e => e.IdMascota).HasColumnName("Id_Mascota");
            entity.Property(e => e.NomCita)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Nom_Cita");

            entity.HasOne(d => d.DocumentoClienteNavigation).WithMany(p => p.CitaExternas)
                .HasForeignKey(d => d.DocumentoCliente)
                .HasConstraintName("FK__cita_exte__Docum__4F7CD00D");

            entity.HasOne(d => d.IdMascotaNavigation).WithMany(p => p.CitaExternas)
                .HasForeignKey(d => d.IdMascota)
                .HasConstraintName("FK__cita_exte__Id_Ma__4E88ABD4");
        });

        modelBuilder.Entity<CitaInterna>(entity =>
        {
            entity.HasKey(e => e.IdCitaInt).HasName("PK__cita_int__499F1189AA330D6D");

            entity.ToTable("cita_internas");

            entity.Property(e => e.IdCitaInt).HasColumnName("Id_Cita_Int");
            entity.Property(e => e.DocumentoCliente).HasColumnName("Documento_Cliente");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FechaHora)
                .HasColumnType("date")
                .HasColumnName("Fecha_Hora");
            entity.Property(e => e.IdMascota).HasColumnName("Id_Mascota");
            entity.Property(e => e.NomCita)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Nom_Cita");

            entity.HasOne(d => d.DocumentoClienteNavigation).WithMany(p => p.CitaInternas)
                .HasForeignKey(d => d.DocumentoCliente)
                .HasConstraintName("FK__cita_inte__Docum__5DCAEF64");

            entity.HasOne(d => d.IdMascotaNavigation).WithMany(p => p.CitaInternas)
                .HasForeignKey(d => d.IdMascota)
                .HasConstraintName("FK__cita_inte__Id_Ma__5CD6CB2B");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.DocumentoCliente).HasName("PK__clientes__B64AC97392296B2D");

            entity.ToTable("clientes");

            entity.Property(e => e.DocumentoCliente)
                .ValueGeneratedNever()
                .HasColumnName("Documento_Cliente");
            entity.Property(e => e.Correo)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.NombreCliente)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Nombre_Cliente");
            entity.Property(e => e.TipoDocu)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("Tipo_Docu");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.IdCompra).HasName("PK__compras__661E0ED012267630");

            entity.ToTable("compras");

            entity.Property(e => e.IdCompra).HasColumnName("Id_Compra");
            entity.Property(e => e.FechaCompra)
                .HasColumnType("date")
                .HasColumnName("Fecha_Compra");
            entity.Property(e => e.FormaPago)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Forma_Pago");
            entity.Property(e => e.Total).HasColumnName("Total");
        });

        modelBuilder.Entity<Configuracion>(entity =>
        {
            entity.HasKey(e => e.IdConfiguracion).HasName("PK__configur__73F5BB5AD557B58D");

            entity.ToTable("configuracion");

            entity.Property(e => e.IdConfiguracion).HasColumnName("Id_Configuracion");
            entity.Property(e => e.IdPermiso).HasColumnName("Id_Permiso");
            entity.Property(e => e.IdRol).HasColumnName("Id_Rol");

            entity.HasOne(d => d.IdPermisoNavigation).WithMany(p => p.Configuracions)
                .HasForeignKey(d => d.IdPermiso)
                .HasConstraintName("FK__configura__Id_Pe__5BE2A6F2");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Configuracions)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__configura__Id_Ro__5AEE82B9");
        });

        modelBuilder.Entity<DetaCompra>(entity =>
        {
            entity.HasKey(e => e.IdDetaCompra).HasName("PK__deta_com__9C865E39DEF429E1");

            entity.ToTable("deta_compras");

            entity.Property(e => e.IdDetaCompra).HasColumnName("Id_Deta_Compra");
            entity.Property(e => e.IdCompra).HasColumnName("Id_Compra");
            entity.Property(e => e.IdProducto).HasColumnName("Id_Producto");
            entity.Property(e => e.PrecioCompra).HasColumnName("Precio_Compra");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.DetaCompras)
                .HasForeignKey(d => d.IdCompra)
                .HasConstraintName("FK__deta_comp__Id_Co__09A971A2");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetaCompras)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__deta_comp__Id_Pr__0A9D95DB");
        });

        

        modelBuilder.Entity<DetaVentum>(entity =>
        {
            entity.HasKey(e => e.IdDetalleV).HasName("PK__deta_ven__540046FFEEFC1F39");

            entity.ToTable("deta_venta");

            entity.Property(e => e.IdDetalleV).HasColumnName("Id_detalle_V");
            entity.Property(e => e.IdProducto).HasColumnName("Id_Producto");
            entity.Property(e => e.IdServicio).HasColumnName("Id_Servicio");
            entity.Property(e => e.IdVenta).HasColumnName("Id_Venta");
            entity.Property(e => e.SubTotalPro).HasColumnName("Sub_Total_Pro");
            entity.Property(e => e.SubTotalSer).HasColumnName("Sub_Total_Ser");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetaVenta)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__deta_vent__Id_Pr__04E4BC85");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.DetaVenta)
                .HasForeignKey(d => d.IdServicio)
                .HasConstraintName("FK__deta_vent__Id_Se__03F0984C");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.DetaVenta)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("FK__deta_vent__Id_Ve__02FC7413");
        });

        modelBuilder.Entity<Mascota>(entity =>
        {
            entity.HasKey(e => e.IdMascota).HasName("PK__mascotas__C7A382FEB5C42430");

            entity.ToTable("mascotas");

            entity.Property(e => e.IdMascota).HasColumnName("Id_Mascota");
            entity.Property(e => e.ColorMascota)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Color_Mascota");
            entity.Property(e => e.DocumentoCliente).HasColumnName("Documento_Cliente");
            entity.Property(e => e.Especie)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FechaNacimiento)
                .HasColumnType("date")
                .HasColumnName("Fecha_Nacimiento");
            entity.Property(e => e.Genero)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.InfMascota)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Inf_Mascota");
            entity.Property(e => e.NombreMascota)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Nombre_Mascota");
            entity.Property(e => e.Raza)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.DocumentoClienteNavigation).WithMany(p => p.Mascota)
                .HasForeignKey(d => d.DocumentoCliente)
                .HasConstraintName("FK__mascotas__Inf_Ma__4BAC3F29");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermiso).HasName("PK__permisos__153CFB6D328BCFAA");

            entity.ToTable("permisos");

            entity.Property(e => e.IdPermiso).HasColumnName("Id_Permiso");
            entity.Property(e => e.NomPermiso)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("Nom_Permiso");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__producto__2085A9CF5B8C577C");

            entity.ToTable("productos");

            entity.Property(e => e.IdProducto).HasColumnName("Id_Producto");
            entity.Property(e => e.Disponibilidad)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.FechaVen)
                .HasColumnType("date")
                .HasColumnName("Fecha_Ven");
            entity.Property(e => e.IdCategoria).HasColumnName("Id_Categoria");
            entity.Property(e => e.IdProveedor).HasColumnName("Id_proveedor");
            entity.Property(e => e.NomProducto)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("Nom_producto");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("FK__productos__Id_Ca__7E37BEF6");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("FK__productos__Id_pr__7D439ABD");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("PK__proveedo__477B858E1EDA75DB");

            entity.ToTable("proveedores");

            entity.Property(e => e.IdProveedor).HasColumnName("Id_Proveedor");
            entity.Property(e => e.Correo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.NitProveedor).HasColumnName("Nit_Proveedor");
            entity.Property(e => e.NomProveedor)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Nom_Proveedor");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__roles__55932E86C5A7B610");

            entity.ToTable("roles");

            entity.Property(e => e.IdRol).HasColumnName("Id_Rol");
            entity.Property(e => e.NomRol)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Nom_Rol");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.IdServicio).HasName("PK__servicio__5B1345F0BD5626CF");

            entity.ToTable("servicios");

            entity.Property(e => e.IdServicio).HasColumnName("Id_Servicio");
            entity.Property(e => e.Categoria)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.NomServico)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Nom_Servico");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__usuario__63C76BE2073CA765");

            entity.ToTable("usuario");

            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.IdRol).HasColumnName("Id_Rol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.TipoDoc)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Tipo_Doc");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__usuario__Id_Rol__5EBF139D");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.IdVenta).HasName("PK__ventas__B3C9EABD9CF5DD9D");

            entity.ToTable("ventas");

            entity.Property(e => e.IdVenta).HasColumnName("Id_Venta");
            entity.Property(e => e.DocumentoCliente).HasColumnName("Documento_Cliente");
            entity.Property(e => e.FechaVenta)
                .HasColumnType("date")
                .HasColumnName("Fecha_Venta");
            entity.Property(e => e.FormaPago)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Forma_Pago");
            entity.Property(e => e.Total).HasColumnName("Total");

            entity.HasOne(d => d.DocumentoClienteNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.DocumentoCliente)
                .HasConstraintName("FK__ventas__Document__74AE54BC");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
