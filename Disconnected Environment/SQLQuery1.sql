create database Praktikum_PABD

create table Prodi
(
id_prodi varchar (15) primary key,
nama_prodi varchar (50) 
)

create table Mahasiswa
(
nim varchar (15) primary key ,
nama_mahasiswa varchar (50),
jenis_kelamin char (1) constraint ck_jk Check (jenis_Kelamin in ('L', 'P')),
alamat varchar (50),
tgl_lahir varchar (10),
id_prodi varchar (15) foreign key references Prodi(id_prodi)
)

create table Status_mahasiswa
(
id_status varchar (15) primary key,
nim varchar (15) foreign key references Mahasiswa(nim),
status_mahasiswa varchar(15),
tahun_masuk varchar (4)
)

--drop table Prodi