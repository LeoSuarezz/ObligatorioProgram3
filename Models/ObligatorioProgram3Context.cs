using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ObligatorioProgram3.Models;

public partial class ObligatorioProgram3Context : DbContext
{
    public ObligatorioProgram3Context()
    {
    }

    public ObligatorioProgram3Context(DbContextOptions<ObligatorioProgram3Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Clima> Climas { get; set; }

    public virtual DbSet<Cotizacion> Cotizacions { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Mesa> Mesas { get; set; }

    public virtual DbSet<OrdenDetalle> OrdenDetalles { get; set; }

    public virtual DbSet<Ordene> Ordenes { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Reseña> Reseñas { get; set; }

    public virtual DbSet<Restaurante> Restaurantes { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }
   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clientes__3214EC27CB2112B0");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Apellido)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.TipoCliente)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Clima>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clima__3214EC27237D7D40");

            entity.ToTable("Clima");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DescripcionClima)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Temperatura).HasColumnType("decimal(2, 2)");
        });

        modelBuilder.Entity<Cotizacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cotizaci__3214EC2707DA1A8E");

            entity.ToTable("Cotizacion");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Moneda)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Menu__3214EC27F47E07B9");

            entity.ToTable("Menu");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.NombrePlato)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.RutaImagen)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Mesa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Mesas__3214EC27B9E8A190");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Estado)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Idrestaurante).HasColumnName("IDRestaurante");

            entity.HasOne(d => d.IdrestauranteNavigation).WithMany(p => p.Mesas)
                .HasForeignKey(d => d.Idrestaurante)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_MesaRestaurante");
        });

        modelBuilder.Entity<OrdenDetalle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrdenDet__3214EC27E638D6B8");

            entity.ToTable("OrdenDetalle");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idmenu).HasColumnName("IDMenu");
            entity.Property(e => e.Idorden).HasColumnName("IDOrden");

            entity.HasOne(d => d.IdmenuNavigation).WithMany(p => p.OrdenDetalles)
                .HasForeignKey(d => d.Idmenu)
                .HasConstraintName("FK_OrdenMenu");

            entity.HasOne(d => d.IdordenNavigation).WithMany(p => p.OrdenDetalles)
                .HasForeignKey(d => d.Idorden)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DetalleOrden");
        });

        modelBuilder.Entity<Ordene>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ordenes__3214EC2730C13D2C");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idreserva).HasColumnName("IDReserva");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdreservaNavigation).WithMany(p => p.Ordenes)
                .HasForeignKey(d => d.Idreserva)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OrdenReserva");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pagos__3214EC2740B14741");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idclima).HasColumnName("IDClima");
            entity.Property(e => e.Idcotizacion).HasColumnName("IDCotizacion");
            entity.Property(e => e.Idreserva).HasColumnName("IDReserva");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdclimaNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.Idclima)
                .HasConstraintName("FK_PagosClima");

            entity.HasOne(d => d.IdcotizacionNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.Idcotizacion)
                .HasConstraintName("FK_PagosCotizacion");

            entity.HasOne(d => d.IdreservaNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.Idreserva)
                .HasConstraintName("FK_PagosReserva");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Permisos__3214EC271EEF490F");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idrol).HasColumnName("IDRol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.IdrolNavigation).WithMany(p => p.Permisos)
                .HasForeignKey(d => d.Idrol)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PermisosRol");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reservas__3214EC272CCFE4F3");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Idcliente).HasColumnName("IDCliente");
            entity.Property(e => e.Idmesa).HasColumnName("IDMesa");

            entity.HasOne(d => d.IdclienteNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.Idcliente)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ReservaClientes");

            entity.HasOne(d => d.IdmesaNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.Idmesa)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ReservaMesas");
        });

        modelBuilder.Entity<Reseña>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reseñas__3214EC27E6058218");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Comentario)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Idcliente).HasColumnName("IDCliente");
            entity.Property(e => e.Idrestaurante).HasColumnName("IDRestaurante");

            entity.HasOne(d => d.IdclienteNavigation).WithMany(p => p.Reseñas)
                .HasForeignKey(d => d.Idcliente)
                .HasConstraintName("FK_ReseñaCliente");

            entity.HasOne(d => d.IdrestauranteNavigation).WithMany(p => p.Reseñas)
                .HasForeignKey(d => d.Idrestaurante)
                .HasConstraintName("FK_ReseñaRestaurante");
        });

        modelBuilder.Entity<Restaurante>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Restaura__3214EC27B34E531D");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rol__3214EC2700BB89D9");

            entity.ToTable("Rol");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC276BDF0841");

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D105349382CA98").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Apellido)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Contraseña)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Idrol).HasColumnName("IDRol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.IdrolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Idrol)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UsuariosRoles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
