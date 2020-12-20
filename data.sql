USE [master]
GO
/****** Object:  Database [WeSplitDB]    Script Date: 20/12/2020 23:01:57 ******/
CREATE DATABASE [WeSplitDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'WeSplitDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\WeSplitDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'WeSplitDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\WeSplitDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [WeSplitDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [WeSplitDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [WeSplitDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [WeSplitDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [WeSplitDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [WeSplitDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [WeSplitDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [WeSplitDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [WeSplitDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [WeSplitDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [WeSplitDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [WeSplitDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [WeSplitDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [WeSplitDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [WeSplitDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [WeSplitDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [WeSplitDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [WeSplitDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [WeSplitDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [WeSplitDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [WeSplitDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [WeSplitDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [WeSplitDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [WeSplitDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [WeSplitDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [WeSplitDB] SET  MULTI_USER 
GO
ALTER DATABASE [WeSplitDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [WeSplitDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [WeSplitDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [WeSplitDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [WeSplitDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [WeSplitDB] SET QUERY_STORE = OFF
GO
USE [WeSplitDB]
GO
/****** Object:  Table [dbo].[Expenses]    Script Date: 20/12/2020 23:01:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Expenses](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[trip_id] [int] NOT NULL,
	[expense] [money] NOT NULL,
	[description] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Expenses] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Joining]    Script Date: 20/12/2020 23:01:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Joining](
	[member_id] [int] NOT NULL,
	[trip_id] [int] NOT NULL,
	[advance_deposit] [money] NOT NULL,
 CONSTRAINT [PK_Joining] PRIMARY KEY CLUSTERED 
(
	[member_id] ASC,
	[trip_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Members]    Script Date: 20/12/2020 23:01:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Members](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[avatar] [varchar](50) NULL,
	[phone] [char](10) NULL,
 CONSTRAINT [PK_Members] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlaceImages]    Script Date: 20/12/2020 23:01:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlaceImages](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[place_id] [int] NOT NULL,
	[file_name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_PlaceImages] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Places]    Script Date: 20/12/2020 23:01:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Places](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[description] [nvarchar](max) NOT NULL,
	[province_id] [int] NOT NULL,
 CONSTRAINT [PK_Places] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Provinces]    Script Date: 20/12/2020 23:01:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Provinces](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Provinces] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RestStops]    Script Date: 20/12/2020 23:01:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RestStops](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[trip_id] [int] NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_RestStops] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TripImages]    Script Date: 20/12/2020 23:01:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TripImages](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[trip_id] [int] NOT NULL,
	[file_name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TripImages] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Trips]    Script Date: 20/12/2020 23:01:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trips](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[departure_date] [date] NOT NULL,
	[return_date] [date] NOT NULL,
	[place_id] [int] NOT NULL,
	[is_finished] [bit] NOT NULL,
	[description] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Trips] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Expenses] ADD  CONSTRAINT [DF_Expenses_expense]  DEFAULT ((0)) FOR [expense]
GO
ALTER TABLE [dbo].[Joining] ADD  CONSTRAINT [DF_Joining_advance_deposit]  DEFAULT ((0)) FOR [advance_deposit]
GO
ALTER TABLE [dbo].[Trips] ADD  CONSTRAINT [DF_Trips_is_finished]  DEFAULT ((0)) FOR [is_finished]
GO
ALTER TABLE [dbo].[Trips] ADD  CONSTRAINT [DF_Trips_description]  DEFAULT ('') FOR [description]
GO
ALTER TABLE [dbo].[Expenses]  WITH CHECK ADD  CONSTRAINT [FK_Expenses_Trips] FOREIGN KEY([trip_id])
REFERENCES [dbo].[Trips] ([id])
GO
ALTER TABLE [dbo].[Expenses] CHECK CONSTRAINT [FK_Expenses_Trips]
GO
ALTER TABLE [dbo].[Joining]  WITH CHECK ADD  CONSTRAINT [FK_Joining_Members] FOREIGN KEY([member_id])
REFERENCES [dbo].[Members] ([id])
GO
ALTER TABLE [dbo].[Joining] CHECK CONSTRAINT [FK_Joining_Members]
GO
ALTER TABLE [dbo].[Joining]  WITH CHECK ADD  CONSTRAINT [FK_Joining_Trips] FOREIGN KEY([trip_id])
REFERENCES [dbo].[Trips] ([id])
GO
ALTER TABLE [dbo].[Joining] CHECK CONSTRAINT [FK_Joining_Trips]
GO
ALTER TABLE [dbo].[PlaceImages]  WITH CHECK ADD  CONSTRAINT [FK_PlaceImages_Places] FOREIGN KEY([place_id])
REFERENCES [dbo].[Places] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PlaceImages] CHECK CONSTRAINT [FK_PlaceImages_Places]
GO
ALTER TABLE [dbo].[Places]  WITH CHECK ADD  CONSTRAINT [FK_Places_Provinces] FOREIGN KEY([province_id])
REFERENCES [dbo].[Provinces] ([id])
GO
ALTER TABLE [dbo].[Places] CHECK CONSTRAINT [FK_Places_Provinces]
GO
ALTER TABLE [dbo].[RestStops]  WITH CHECK ADD  CONSTRAINT [FK_RestStops_Trips] FOREIGN KEY([trip_id])
REFERENCES [dbo].[Trips] ([id])
GO
ALTER TABLE [dbo].[RestStops] CHECK CONSTRAINT [FK_RestStops_Trips]
GO
ALTER TABLE [dbo].[TripImages]  WITH CHECK ADD  CONSTRAINT [FK_TripImages_Trips] FOREIGN KEY([trip_id])
REFERENCES [dbo].[Trips] ([id])
GO
ALTER TABLE [dbo].[TripImages] CHECK CONSTRAINT [FK_TripImages_Trips]
GO
ALTER TABLE [dbo].[Trips]  WITH CHECK ADD  CONSTRAINT [FK_Trips_Places] FOREIGN KEY([place_id])
REFERENCES [dbo].[Places] ([id])
GO
ALTER TABLE [dbo].[Trips] CHECK CONSTRAINT [FK_Trips_Places]
GO
USE [master]
GO
ALTER DATABASE [WeSplitDB] SET  READ_WRITE 
GO
