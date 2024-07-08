﻿using System;
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

    public virtual DbSet<Rol> Rol { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }
    public IEnumerable<object> RolPermiso { get; internal set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clientes__3214EC27AAE9DA96");

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
            entity.HasKey(e => e.Id).HasName("PK__Clima__3214EC27180A4653");

            entity.ToTable("Clima");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DescripcionClima)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Temperatura).HasColumnType("float");
        });

        modelBuilder.Entity<Cotizacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cotizaci__3214EC27815FD456");

            entity.ToTable("Cotizacion");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Moneda)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Menu__3214EC278613F335");

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
            entity.Property(e => e.Categoria).HasMaxLength(30);
        });

        modelBuilder.Entity<Mesa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Mesas__3214EC27EEDBF2F1");

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
            entity.HasKey(e => e.Id).HasName("PK__OrdenDet__3214EC27293E9D4B");

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
            entity.HasKey(e => e.Id).HasName("PK__Ordenes__3214EC2760805297");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idreserva).HasColumnName("IDReserva");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Estado)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.IdreservaNavigation).WithMany(p => p.Ordenes)
                .HasForeignKey(d => d.Idreserva)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OrdenReserva");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pagos__3214EC27A3E14295");

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

       

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reservas__3214EC271470247F");

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
            entity.HasKey(e => e.Id).HasName("PK__Reseñas__3214EC277E3C5AC0");

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
            entity.HasKey(e => e.Id).HasName("PK__Restaura__3214EC27D6EF9985");

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

        
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC2707E35BEF");

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D10534CAEBD9BC").IsUnique();

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
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_UsuariosRoles");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.NombreRol)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasMany(e => e.Usuarios)
                .WithOne(u => u.IdrolNavigation)
                .HasForeignKey(u => u.Idrol);

            entity.HasMany(e => e.IdPermisos)
                .WithMany(p => p.IdRols)
                .UsingEntity<RolPermiso>(
                    j => j
                        .HasOne(rp => rp.Permiso)
                        .WithMany(p => p.RolPermisos)
                        .HasForeignKey(rp => rp.IdPermisos),
                    j => j
                        .HasOne(rp => rp.Rol)
                        .WithMany(r => r.RolPermisos)
                        .HasForeignKey(rp => rp.IdRol),
                    j =>
                    {
                        j.HasKey(rp => new { rp.IdRol, rp.IdPermisos });
                        j.ToTable("RolPermiso");
                    });
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100);

        });

        modelBuilder.Entity<RolPermiso>(entity =>
        {
            entity.HasKey(e => new { e.IdRol, e.IdPermisos });

            entity.HasOne(d => d.Rol)
                .WithMany(p => p.RolPermisos)
                .HasForeignKey(d => d.IdRol);

            entity.HasOne(d => d.Permiso)
                .WithMany(p => p.RolPermisos)
                .HasForeignKey(d => d.IdPermisos);

        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
