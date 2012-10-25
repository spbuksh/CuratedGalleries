﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Corbis.DB.Linq
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Corbis.Main")]
	public partial class MainDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertAdminUserMembershipRecord(AdminUserMembershipRecord instance);
    partial void UpdateAdminUserMembershipRecord(AdminUserMembershipRecord instance);
    partial void DeleteAdminUserMembershipRecord(AdminUserMembershipRecord instance);
    partial void InsertAdminUserRoleRecord(AdminUserRoleRecord instance);
    partial void UpdateAdminUserRoleRecord(AdminUserRoleRecord instance);
    partial void DeleteAdminUserRoleRecord(AdminUserRoleRecord instance);
    partial void InsertFileRecord(FileRecord instance);
    partial void UpdateFileRecord(FileRecord instance);
    partial void DeleteFileRecord(FileRecord instance);
    partial void InsertCuratedGalleryRecord(CuratedGalleryRecord instance);
    partial void UpdateCuratedGalleryRecord(CuratedGalleryRecord instance);
    partial void DeleteCuratedGalleryRecord(CuratedGalleryRecord instance);
    partial void InsertAdminUserProfileRecord(AdminUserProfileRecord instance);
    partial void UpdateAdminUserProfileRecord(AdminUserProfileRecord instance);
    partial void DeleteAdminUserProfileRecord(AdminUserProfileRecord instance);
    partial void InsertGalleryTemplateRecord(GalleryTemplateRecord instance);
    partial void UpdateGalleryTemplateRecord(GalleryTemplateRecord instance);
    partial void DeleteGalleryTemplateRecord(GalleryTemplateRecord instance);
    #endregion
		
		public MainDataContext() : 
				base(global::Corbis.DB.Linq.Properties.Settings.Default.Corbis_MainConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public MainDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public MainDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public MainDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public MainDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<AdminUserMembershipRecord> AdminUserMembershipRecords
		{
			get
			{
				return this.GetTable<AdminUserMembershipRecord>();
			}
		}
		
		public System.Data.Linq.Table<AdminUserRoleRecord> AdminUserRoleRecords
		{
			get
			{
				return this.GetTable<AdminUserRoleRecord>();
			}
		}
		
		public System.Data.Linq.Table<AdminUserToRoleRecord> AdminUserToRoleRecords
		{
			get
			{
				return this.GetTable<AdminUserToRoleRecord>();
			}
		}
		
		public System.Data.Linq.Table<FileRecord> FileRecords
		{
			get
			{
				return this.GetTable<FileRecord>();
			}
		}
		
		public System.Data.Linq.Table<CuratedGalleryRecord> CuratedGalleryRecords
		{
			get
			{
				return this.GetTable<CuratedGalleryRecord>();
			}
		}
		
		public System.Data.Linq.Table<AdminUserProfileRecord> AdminUserProfileRecords
		{
			get
			{
				return this.GetTable<AdminUserProfileRecord>();
			}
		}
		
		public System.Data.Linq.Table<GalleryTemplateRecord> GalleryTemplateRecords
		{
			get
			{
				return this.GetTable<GalleryTemplateRecord>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.AdminUserMembership")]
	public partial class AdminUserMembershipRecord : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ID;
		
		private int _ProfileID;
		
		private string _Login;
		
		private string _Password;
		
		private System.Nullable<System.DateTime> _PasswordExpirationDate;
		
		private bool _IsActive;
		
		private System.DateTime _DateCreated;
		
		private EntityRef<AdminUserProfileRecord> _AdminUserProfileRecord;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(int value);
    partial void OnIDChanged();
    partial void OnProfileIDChanging(int value);
    partial void OnProfileIDChanged();
    partial void OnLoginChanging(string value);
    partial void OnLoginChanged();
    partial void OnPasswordChanging(string value);
    partial void OnPasswordChanged();
    partial void OnPasswordExpirationDateChanging(System.Nullable<System.DateTime> value);
    partial void OnPasswordExpirationDateChanged();
    partial void OnIsActiveChanging(bool value);
    partial void OnIsActiveChanged();
    partial void OnDateCreatedChanging(System.DateTime value);
    partial void OnDateCreatedChanged();
    #endregion
		
		public AdminUserMembershipRecord()
		{
			this._AdminUserProfileRecord = default(EntityRef<AdminUserProfileRecord>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProfileID", DbType="Int NOT NULL")]
		public int ProfileID
		{
			get
			{
				return this._ProfileID;
			}
			set
			{
				if ((this._ProfileID != value))
				{
					if (this._AdminUserProfileRecord.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnProfileIDChanging(value);
					this.SendPropertyChanging();
					this._ProfileID = value;
					this.SendPropertyChanged("ProfileID");
					this.OnProfileIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Login", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string Login
		{
			get
			{
				return this._Login;
			}
			set
			{
				if ((this._Login != value))
				{
					this.OnLoginChanging(value);
					this.SendPropertyChanging();
					this._Login = value;
					this.SendPropertyChanged("Login");
					this.OnLoginChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Password", DbType="NVarChar(150) NOT NULL", CanBeNull=false)]
		public string Password
		{
			get
			{
				return this._Password;
			}
			set
			{
				if ((this._Password != value))
				{
					this.OnPasswordChanging(value);
					this.SendPropertyChanging();
					this._Password = value;
					this.SendPropertyChanged("Password");
					this.OnPasswordChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PasswordExpirationDate", DbType="DateTime2")]
		public System.Nullable<System.DateTime> PasswordExpirationDate
		{
			get
			{
				return this._PasswordExpirationDate;
			}
			set
			{
				if ((this._PasswordExpirationDate != value))
				{
					this.OnPasswordExpirationDateChanging(value);
					this.SendPropertyChanging();
					this._PasswordExpirationDate = value;
					this.SendPropertyChanged("PasswordExpirationDate");
					this.OnPasswordExpirationDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsActive", DbType="Bit NOT NULL")]
		public bool IsActive
		{
			get
			{
				return this._IsActive;
			}
			set
			{
				if ((this._IsActive != value))
				{
					this.OnIsActiveChanging(value);
					this.SendPropertyChanging();
					this._IsActive = value;
					this.SendPropertyChanged("IsActive");
					this.OnIsActiveChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DateCreated", DbType="DateTime2 NOT NULL")]
		public System.DateTime DateCreated
		{
			get
			{
				return this._DateCreated;
			}
			set
			{
				if ((this._DateCreated != value))
				{
					this.OnDateCreatedChanging(value);
					this.SendPropertyChanging();
					this._DateCreated = value;
					this.SendPropertyChanged("DateCreated");
					this.OnDateCreatedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="AdminUserProfileRecord_AdminUserMembershipRecord", Storage="_AdminUserProfileRecord", ThisKey="ProfileID", OtherKey="ID", IsForeignKey=true, DeleteOnNull=true, DeleteRule="CASCADE")]
		public AdminUserProfileRecord AdminUserProfileRecord
		{
			get
			{
				return this._AdminUserProfileRecord.Entity;
			}
			set
			{
				AdminUserProfileRecord previousValue = this._AdminUserProfileRecord.Entity;
				if (((previousValue != value) 
							|| (this._AdminUserProfileRecord.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._AdminUserProfileRecord.Entity = null;
						previousValue.AdminUserMembershipRecords.Remove(this);
					}
					this._AdminUserProfileRecord.Entity = value;
					if ((value != null))
					{
						value.AdminUserMembershipRecords.Add(this);
						this._ProfileID = value.ID;
					}
					else
					{
						this._ProfileID = default(int);
					}
					this.SendPropertyChanged("AdminUserProfileRecord");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.AdminUserRole")]
	public partial class AdminUserRoleRecord : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ID;
		
		private string _Name;
		
		private string _Description;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(int value);
    partial void OnIDChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnDescriptionChanging(string value);
    partial void OnDescriptionChanged();
    #endregion
		
		public AdminUserRoleRecord()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Description", DbType="NVarChar(255)")]
		public string Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				if ((this._Description != value))
				{
					this.OnDescriptionChanging(value);
					this.SendPropertyChanging();
					this._Description = value;
					this.SendPropertyChanged("Description");
					this.OnDescriptionChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.AdminUserToRole")]
	public partial class AdminUserToRoleRecord
	{
		
		private int _MemberID;
		
		private int _RoleID;
		
		public AdminUserToRoleRecord()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MemberID", DbType="Int NOT NULL")]
		public int MemberID
		{
			get
			{
				return this._MemberID;
			}
			set
			{
				if ((this._MemberID != value))
				{
					this._MemberID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RoleID", DbType="Int NOT NULL")]
		public int RoleID
		{
			get
			{
				return this._RoleID;
			}
			set
			{
				if ((this._RoleID != value))
				{
					this._RoleID = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.[File]")]
	public partial class FileRecord : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _ID;
		
		private string _Name;
		
		private System.Data.Linq.Binary _Content;
		
		private EntitySet<CuratedGalleryRecord> _CuratedGalleryRecords;
		
		private EntitySet<GalleryTemplateRecord> _GalleryTemplateRecords;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(long value);
    partial void OnIDChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnContentChanging(System.Data.Linq.Binary value);
    partial void OnContentChanged();
    #endregion
		
		public FileRecord()
		{
			this._CuratedGalleryRecords = new EntitySet<CuratedGalleryRecord>(new Action<CuratedGalleryRecord>(this.attach_CuratedGalleryRecords), new Action<CuratedGalleryRecord>(this.detach_CuratedGalleryRecords));
			this._GalleryTemplateRecords = new EntitySet<GalleryTemplateRecord>(new Action<GalleryTemplateRecord>(this.attach_GalleryTemplateRecords), new Action<GalleryTemplateRecord>(this.detach_GalleryTemplateRecords));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NChar(50)")]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Content", DbType="VarBinary(MAX) NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public System.Data.Linq.Binary Content
		{
			get
			{
				return this._Content;
			}
			set
			{
				if ((this._Content != value))
				{
					this.OnContentChanging(value);
					this.SendPropertyChanging();
					this._Content = value;
					this.SendPropertyChanged("Content");
					this.OnContentChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="FileRecord_CuratedGalleryRecord", Storage="_CuratedGalleryRecords", ThisKey="ID", OtherKey="Archive")]
		public EntitySet<CuratedGalleryRecord> CuratedGalleryRecords
		{
			get
			{
				return this._CuratedGalleryRecords;
			}
			set
			{
				this._CuratedGalleryRecords.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="FileRecord_GalleryTemplate", Storage="_GalleryTemplateRecords", ThisKey="ID", OtherKey="PackageID")]
		public EntitySet<GalleryTemplateRecord> GalleryTemplateRecords
		{
			get
			{
				return this._GalleryTemplateRecords;
			}
			set
			{
				this._GalleryTemplateRecords.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_CuratedGalleryRecords(CuratedGalleryRecord entity)
		{
			this.SendPropertyChanging();
			entity.FileRecord = this;
		}
		
		private void detach_CuratedGalleryRecords(CuratedGalleryRecord entity)
		{
			this.SendPropertyChanging();
			entity.FileRecord = null;
		}
		
		private void attach_GalleryTemplateRecords(GalleryTemplateRecord entity)
		{
			this.SendPropertyChanging();
			entity.FileRecord = this;
		}
		
		private void detach_GalleryTemplateRecords(GalleryTemplateRecord entity)
		{
			this.SendPropertyChanging();
			entity.FileRecord = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.CuratedGallery")]
	public partial class CuratedGalleryRecord : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ID;
		
		private string _Name;
		
		private bool _Enabled;
		
		private int _TemplateID;
		
		private System.DateTime _DateCreated;
		
		private System.Nullable<System.DateTime> _DateModified;
		
		private System.Nullable<long> _Archive;
		
		private EntityRef<FileRecord> _FileRecord;
		
		private EntityRef<GalleryTemplateRecord> _GalleryTemplateRecord;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(int value);
    partial void OnIDChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnEnabledChanging(bool value);
    partial void OnEnabledChanged();
    partial void OnTemplateIDChanging(int value);
    partial void OnTemplateIDChanged();
    partial void OnDateCreatedChanging(System.DateTime value);
    partial void OnDateCreatedChanged();
    partial void OnDateModifiedChanging(System.Nullable<System.DateTime> value);
    partial void OnDateModifiedChanged();
    partial void OnArchiveChanging(System.Nullable<long> value);
    partial void OnArchiveChanged();
    #endregion
		
		public CuratedGalleryRecord()
		{
			this._FileRecord = default(EntityRef<FileRecord>);
			this._GalleryTemplateRecord = default(EntityRef<GalleryTemplateRecord>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Enabled", DbType="Bit NOT NULL")]
		public bool Enabled
		{
			get
			{
				return this._Enabled;
			}
			set
			{
				if ((this._Enabled != value))
				{
					this.OnEnabledChanging(value);
					this.SendPropertyChanging();
					this._Enabled = value;
					this.SendPropertyChanged("Enabled");
					this.OnEnabledChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TemplateID", DbType="Int NOT NULL")]
		public int TemplateID
		{
			get
			{
				return this._TemplateID;
			}
			set
			{
				if ((this._TemplateID != value))
				{
					if (this._GalleryTemplateRecord.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnTemplateIDChanging(value);
					this.SendPropertyChanging();
					this._TemplateID = value;
					this.SendPropertyChanged("TemplateID");
					this.OnTemplateIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DateCreated", DbType="DateTime2 NOT NULL")]
		public System.DateTime DateCreated
		{
			get
			{
				return this._DateCreated;
			}
			set
			{
				if ((this._DateCreated != value))
				{
					this.OnDateCreatedChanging(value);
					this.SendPropertyChanging();
					this._DateCreated = value;
					this.SendPropertyChanged("DateCreated");
					this.OnDateCreatedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DateModified", DbType="DateTime2")]
		public System.Nullable<System.DateTime> DateModified
		{
			get
			{
				return this._DateModified;
			}
			set
			{
				if ((this._DateModified != value))
				{
					this.OnDateModifiedChanging(value);
					this.SendPropertyChanging();
					this._DateModified = value;
					this.SendPropertyChanged("DateModified");
					this.OnDateModifiedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Archive", DbType="BigInt")]
		public System.Nullable<long> Archive
		{
			get
			{
				return this._Archive;
			}
			set
			{
				if ((this._Archive != value))
				{
					if (this._FileRecord.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnArchiveChanging(value);
					this.SendPropertyChanging();
					this._Archive = value;
					this.SendPropertyChanged("Archive");
					this.OnArchiveChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="FileRecord_CuratedGalleryRecord", Storage="_FileRecord", ThisKey="Archive", OtherKey="ID", IsForeignKey=true)]
		public FileRecord FileRecord
		{
			get
			{
				return this._FileRecord.Entity;
			}
			set
			{
				FileRecord previousValue = this._FileRecord.Entity;
				if (((previousValue != value) 
							|| (this._FileRecord.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._FileRecord.Entity = null;
						previousValue.CuratedGalleryRecords.Remove(this);
					}
					this._FileRecord.Entity = value;
					if ((value != null))
					{
						value.CuratedGalleryRecords.Add(this);
						this._Archive = value.ID;
					}
					else
					{
						this._Archive = default(Nullable<long>);
					}
					this.SendPropertyChanged("FileRecord");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="GalleryTemplate_CuratedGalleryRecord", Storage="_GalleryTemplateRecord", ThisKey="TemplateID", OtherKey="ID", IsForeignKey=true)]
		public GalleryTemplateRecord GalleryTemplateRecord
		{
			get
			{
				return this._GalleryTemplateRecord.Entity;
			}
			set
			{
				GalleryTemplateRecord previousValue = this._GalleryTemplateRecord.Entity;
				if (((previousValue != value) 
							|| (this._GalleryTemplateRecord.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._GalleryTemplateRecord.Entity = null;
						previousValue.CuratedGalleryRecords.Remove(this);
					}
					this._GalleryTemplateRecord.Entity = value;
					if ((value != null))
					{
						value.CuratedGalleryRecords.Add(this);
						this._TemplateID = value.ID;
					}
					else
					{
						this._TemplateID = default(int);
					}
					this.SendPropertyChanged("GalleryTemplateRecord");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.AdminUserProfile")]
	public partial class AdminUserProfileRecord : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ID;
		
		private string _FirstName;
		
		private string _MiddleName;
		
		private string _LastName;
		
		private string _Email;
		
		private EntitySet<AdminUserMembershipRecord> _AdminUserMembershipRecords;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(int value);
    partial void OnIDChanged();
    partial void OnFirstNameChanging(string value);
    partial void OnFirstNameChanged();
    partial void OnMiddleNameChanging(string value);
    partial void OnMiddleNameChanged();
    partial void OnLastNameChanging(string value);
    partial void OnLastNameChanged();
    partial void OnEmailChanging(string value);
    partial void OnEmailChanged();
    #endregion
		
		public AdminUserProfileRecord()
		{
			this._AdminUserMembershipRecords = new EntitySet<AdminUserMembershipRecord>(new Action<AdminUserMembershipRecord>(this.attach_AdminUserMembershipRecords), new Action<AdminUserMembershipRecord>(this.detach_AdminUserMembershipRecords));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FirstName", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string FirstName
		{
			get
			{
				return this._FirstName;
			}
			set
			{
				if ((this._FirstName != value))
				{
					this.OnFirstNameChanging(value);
					this.SendPropertyChanging();
					this._FirstName = value;
					this.SendPropertyChanged("FirstName");
					this.OnFirstNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MiddleName", DbType="NVarChar(50)")]
		public string MiddleName
		{
			get
			{
				return this._MiddleName;
			}
			set
			{
				if ((this._MiddleName != value))
				{
					this.OnMiddleNameChanging(value);
					this.SendPropertyChanging();
					this._MiddleName = value;
					this.SendPropertyChanged("MiddleName");
					this.OnMiddleNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LastName", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string LastName
		{
			get
			{
				return this._LastName;
			}
			set
			{
				if ((this._LastName != value))
				{
					this.OnLastNameChanging(value);
					this.SendPropertyChanging();
					this._LastName = value;
					this.SendPropertyChanged("LastName");
					this.OnLastNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Email", DbType="NVarChar(100)")]
		public string Email
		{
			get
			{
				return this._Email;
			}
			set
			{
				if ((this._Email != value))
				{
					this.OnEmailChanging(value);
					this.SendPropertyChanging();
					this._Email = value;
					this.SendPropertyChanged("Email");
					this.OnEmailChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="AdminUserProfileRecord_AdminUserMembershipRecord", Storage="_AdminUserMembershipRecords", ThisKey="ID", OtherKey="ProfileID")]
		public EntitySet<AdminUserMembershipRecord> AdminUserMembershipRecords
		{
			get
			{
				return this._AdminUserMembershipRecords;
			}
			set
			{
				this._AdminUserMembershipRecords.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_AdminUserMembershipRecords(AdminUserMembershipRecord entity)
		{
			this.SendPropertyChanging();
			entity.AdminUserProfileRecord = this;
		}
		
		private void detach_AdminUserMembershipRecords(AdminUserMembershipRecord entity)
		{
			this.SendPropertyChanging();
			entity.AdminUserProfileRecord = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.GalleryTemplate")]
	public partial class GalleryTemplateRecord : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ID;
		
		private bool _Enabled;
		
		private long _PackageID;
		
		private bool _IsDefault;
		
		private System.DateTime _DateCreated;
		
		private EntitySet<CuratedGalleryRecord> _CuratedGalleryRecords;
		
		private EntityRef<FileRecord> _FileRecord;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(int value);
    partial void OnIDChanged();
    partial void OnEnabledChanging(bool value);
    partial void OnEnabledChanged();
    partial void OnPackageIDChanging(long value);
    partial void OnPackageIDChanged();
    partial void OnIsDefaultChanging(bool value);
    partial void OnIsDefaultChanged();
    partial void OnDateCreatedChanging(System.DateTime value);
    partial void OnDateCreatedChanged();
    #endregion
		
		public GalleryTemplateRecord()
		{
			this._CuratedGalleryRecords = new EntitySet<CuratedGalleryRecord>(new Action<CuratedGalleryRecord>(this.attach_CuratedGalleryRecords), new Action<CuratedGalleryRecord>(this.detach_CuratedGalleryRecords));
			this._FileRecord = default(EntityRef<FileRecord>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Enabled", DbType="Bit NOT NULL")]
		public bool Enabled
		{
			get
			{
				return this._Enabled;
			}
			set
			{
				if ((this._Enabled != value))
				{
					this.OnEnabledChanging(value);
					this.SendPropertyChanging();
					this._Enabled = value;
					this.SendPropertyChanged("Enabled");
					this.OnEnabledChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PackageID", DbType="BigInt NOT NULL")]
		public long PackageID
		{
			get
			{
				return this._PackageID;
			}
			set
			{
				if ((this._PackageID != value))
				{
					if (this._FileRecord.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnPackageIDChanging(value);
					this.SendPropertyChanging();
					this._PackageID = value;
					this.SendPropertyChanged("PackageID");
					this.OnPackageIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsDefault", DbType="Bit NOT NULL")]
		public bool IsDefault
		{
			get
			{
				return this._IsDefault;
			}
			set
			{
				if ((this._IsDefault != value))
				{
					this.OnIsDefaultChanging(value);
					this.SendPropertyChanging();
					this._IsDefault = value;
					this.SendPropertyChanged("IsDefault");
					this.OnIsDefaultChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DateCreated", DbType="DateTime2 NOT NULL")]
		public System.DateTime DateCreated
		{
			get
			{
				return this._DateCreated;
			}
			set
			{
				if ((this._DateCreated != value))
				{
					this.OnDateCreatedChanging(value);
					this.SendPropertyChanging();
					this._DateCreated = value;
					this.SendPropertyChanged("DateCreated");
					this.OnDateCreatedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="GalleryTemplate_CuratedGalleryRecord", Storage="_CuratedGalleryRecords", ThisKey="ID", OtherKey="TemplateID")]
		public EntitySet<CuratedGalleryRecord> CuratedGalleryRecords
		{
			get
			{
				return this._CuratedGalleryRecords;
			}
			set
			{
				this._CuratedGalleryRecords.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="FileRecord_GalleryTemplate", Storage="_FileRecord", ThisKey="PackageID", OtherKey="ID", IsForeignKey=true)]
		public FileRecord FileRecord
		{
			get
			{
				return this._FileRecord.Entity;
			}
			set
			{
				FileRecord previousValue = this._FileRecord.Entity;
				if (((previousValue != value) 
							|| (this._FileRecord.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._FileRecord.Entity = null;
						previousValue.GalleryTemplateRecords.Remove(this);
					}
					this._FileRecord.Entity = value;
					if ((value != null))
					{
						value.GalleryTemplateRecords.Add(this);
						this._PackageID = value.ID;
					}
					else
					{
						this._PackageID = default(long);
					}
					this.SendPropertyChanged("FileRecord");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_CuratedGalleryRecords(CuratedGalleryRecord entity)
		{
			this.SendPropertyChanging();
			entity.GalleryTemplateRecord = this;
		}
		
		private void detach_CuratedGalleryRecords(CuratedGalleryRecord entity)
		{
			this.SendPropertyChanging();
			entity.GalleryTemplateRecord = null;
		}
	}
}
#pragma warning restore 1591
