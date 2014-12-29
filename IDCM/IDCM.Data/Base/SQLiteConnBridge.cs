using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace IDCM.Data.Base
{
    /// <summary>
    /// SQLiteConnection的桥接实现类
    /// 说明：
    /// 1.用以封装SQLiteConnection的部分方法，转由SQLiteConnectPicker提供支持。
    /// 2.对外部程序集SQLiteConnection的具体调用有此类实例进行桥接实现。
    /// @author JiahaiWu 2014-12-29
    /// </summary>
    class SQLiteConnBridge
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="sconn"></param>
        public SQLiteConnBridge(SQLiteConnection sconn)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(sconn!=null);
#endif
            this._Connection=sconn;
        }
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="connStr"></param>
        public SQLiteConnBridge(string connStr)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(connStr != null);
#endif
            _Connection = new SQLiteConnection();
            _Connection.ConnectionString = connStr;
        }

        // 摘要:
        //     Returns non-zero if the given database connection is in autocommit mode.
        //      Autocommit mode is on by default. Autocommit mode is disabled by a BEGIN
        //     statement. Autocommit mode is re-enabled by a COMMIT or ROLLBACK.
        public bool AutoCommit {
            get
            {
                return _Connection.AutoCommit;
            }
        }
        //
        // 摘要:
        //     Returns the number of rows changed by the last INSERT, UPDATE, or DELETE
        //     statement executed on this connection.
        public int Changes
        {
            get
            {
                return _Connection.Changes;
            }
        }
        //
        // 摘要:
        //     This property is used to obtain or set the custom connection pool implementation
        //     to use, if any. Setting this property to null will cause the default connection
        //     pool implementation to be used.
        internal static ISQLiteConnectionPool ConnectionPool
        { 
            get{return SQLiteConnection.ConnectionPool;}
            set{SQLiteConnection.ConnectionPool=value;}
        }
        //
        // 摘要:
        //     The connection string containing the parameters for the connection
        //
        // 备注:
        //      Parameter Values Required Default Data Source This may be a file name, the
        //     string ":memory:", or any supported URI (starting with SQLite 3.7.7).  Starting
        //     with release 1.0.86.0, in order to use more than one consecutive backslash
        //     (e.g. for a UNC path), each of the adjoining backslash characters must be
        //     doubled (e.g. "\\Network\Share\test.db" would become "\\\\Network\Share\test.db").
        //      Y Version 3 N 3 UseUTF16Encoding TrueFalse N False DateTimeFormat Ticks
        //     - Use the value of DateTime.Ticks.  ISO8601 - Use the ISO-8601 format. Uses
        //     the "yyyy-MM-dd HH:mm:ss.FFFFFFFK" format for UTC DateTime values and "yyyy-MM-dd
        //     HH:mm:ss.FFFFFFF" format for local DateTime values).  JulianDay - The interval
        //     of time in days and fractions of a day since January 1, 4713 BC.  UnixEpoch
        //     - The whole number of seconds since the Unix epoch (January 1, 1970).  InvariantCulture
        //     - Any culture-independent string value that the .NET Framework can interpret
        //     as a valid DateTime.  CurrentCulture - Any string value that the .NET Framework
        //     can interpret as a valid DateTime using the current culture.  N ISO8601 DateTimeKind
        //     Unspecified - Not specified as either UTC or local time.Utc - The time represented
        //     is UTC.Local - The time represented is local time.  N Unspecified DateTimeFormatString
        //     The exact DateTime format string to use for all formatting and parsing of
        //     all DateTime values for this connection.  N null BaseSchemaName Some base
        //     data classes in the framework (e.g. those that build SQL queries dynamically)
        //     assume that an ADO.NET provider cannot support an alternate catalog (i.e.
        //     database) without supporting alternate schemas as well; however, SQLite does
        //     not fit into this model. Therefore, this value is used as a placeholder and
        //     removed prior to preparing any SQL statements that may contain it.  N sqlite_default_schema
        //     BinaryGUID True - Store GUID columns in binary formFalse - Store GUID columns
        //     as text N True Cache Size {size in bytes} N 2000 Synchronous Normal - Normal
        //     file flushing behaviorFull - Full flushing after all writesOff - Underlying
        //     OS flushes I/O's N Full Page Size {size in bytes} N 1024 Password {password}
        //     - Using this parameter requires that the CryptoAPI based codec be enabled
        //     at compile-time for both the native interop assembly and the core managed
        //     assemblies; otherwise, using this parameter may result in an exception being
        //     thrown when attempting to open the connection.  N HexPassword {hexPassword}
        //     - Must contain a sequence of zero or more hexadecimal encoded byte values
        //     without a leading "0x" prefix. Using this parameter requires that the CryptoAPI
        //     based codec be enabled at compile-time for both the native interop assembly
        //     and the core managed assemblies; otherwise, using this parameter may result
        //     in an exception being thrown when attempting to open the connection.  N Enlist
        //     Y - Automatically enlist in distributed transactionsN - No automatic enlistment
        //     N Y Pooling True - Use connection pooling.  False - Do not use connection
        //     pooling.  WARNING: When using the default connection pool implementation,
        //     setting this property to True should be avoided by applications that make
        //     use of COM (either directly or indirectly) due to possible deadlocks that
        //     can occur during the finalization of some COM objects.  N False FailIfMissing
        //     True - Don't create the database if it does not exist, throw an error insteadFalse
        //     - Automatically create the database if it does not exist N False Max Page
        //     Count {size in pages} - Limits the maximum number of pages (limits the size)
        //     of the database N 0 Legacy Format True - Use the more compatible legacy 3.x
        //     database formatFalse - Use the newer 3.3x database format which compresses
        //     numbers more effectively N False Default Timeout {time in seconds}The default
        //     command timeout N 30 Journal Mode Delete - Delete the journal file after
        //     a commitPersist - Zero out and leave the journal file on disk after a commitOff
        //     - Disable the rollback journal entirely N Delete Read Only True - Open the
        //     database for read only accessFalse - Open the database for normal read/write
        //     access N False Max Pool Size The maximum number of connections for the given
        //     connection string that can be in the connection pool N 100 Default IsolationLevel
        //     The default transaciton isolation level N Serializable Foreign Keys Enable
        //     foreign key constraints N False Flags Extra behavioral flags for the connection.
        //     See the System.Data.SQLite.SQLiteConnectionFlags enumeration for possible
        //     values.  N Default SetDefaults True - Apply the default connection settings
        //     to the opened database.  False - Skip applying the default connection settings
        //     to the opened database.  N True ToFullPath True - Attempt to expand the data
        //     source file name to a fully qualified path before opening.  False - Skip
        //     attempting to expand the data source file name to a fully qualified path
        //     before opening.  N True
        public string ConnectionString { 
            get{
                return this._Connection.ConnectionString;
            }
            set{
                this._Connection.ConnectionString = value;
            } 
        }
        //
        // 摘要:
        //     Returns the string "main".
        internal string Database
        {
            get {
                return this._Connection.Database;
            }
        }
        //
        // 摘要:
        //     Returns the data source file name without extension or path.
        public string DataSource {
            get
            {
                return this._Connection.Database;
            }
        }
        //
        // 摘要:
        //     Gets/sets the default command timeout for newly-created commands. This is
        //     especially useful for commands used internally such as inside a SQLiteTransaction,
        //     where setting the timeout is not possible.  This can also be set in the ConnectionString
        //     with "Default Timeout"
        public int DefaultTimeout {
            get
            {
                return this._Connection.DefaultTimeout;
            }
            set
            {
                this._Connection.DefaultTimeout = value;
            }
        }
        //
        // 摘要:
        //     Gets/sets the default database type name for this connection. This value
        //     will only be used when not null.
        public string DefaultTypeName
        {
            get
            {
                return this._Connection.DefaultTypeName;
            }
            set
            {
                this._Connection.DefaultTypeName = value;
            }
        }
        //
        // 摘要:
        //     Returns a string containing the define constants (i.e. compile-time options)
        //     used to compile the core managed assembly, delimited with spaces.
        public static string DefineConstants {
            get
            {
                return SQLiteConnection.DefineConstants;
            }
        }
        //
        // 摘要:
        //     Gets/sets the extra behavioral flags for this connection. See the System.Data.SQLite.SQLiteConnectionFlags
        //     enumeration for a list of possible values.
        public SQLiteConnectionFlags Flags {
            get
            {
                return this._Connection.Flags;
            }
            set
            {
                this._Connection.Flags = value;
            }
        }
        //
        // 摘要:
        //     Returns the rowid of the most recent successful INSERT into the database
        //     from this connection.
        public long LastInsertRowId {
            get
            {
                return _Connection.LastInsertRowId;
            }
        }
        //
        // 摘要:
        //     Returns the maximum amount of memory (in bytes) used by the SQLite core library
        //     since the high-water mark was last reset.
        public long MemoryHighwater {
            get
            {
                return _Connection.MemoryHighwater;
            }
        }
        //
        // 摘要:
        //     Returns the amount of memory (in bytes) currently in use by the SQLite core
        //     library.
        public long MemoryUsed {
            get
            {
                return _Connection.MemoryUsed;
            }
        }
        //
        // 摘要:
        //     Returns non-zero if the underlying native connection handle is owned by this
        //     instance.
        public bool OwnHandle {
            get
            {
                return _Connection.OwnHandle;
            }
        }
        //
        // 摘要:
        //     Non-zero if the built-in (i.e. framework provided) connection string parser
        //     should be used when opening the connection.
        internal bool ParseViaFramework
        {
            get
            {
                return this._Connection.ParseViaFramework;
            }
            set
            {
                this._Connection.ParseViaFramework = value;
            }
        }
        //
        // 摘要:
        //     Returns the number of pool entries for the file name associated with this
        //     connection.
        public int PoolCount {
            get
            {
                return _Connection.PoolCount;
            }
        }
        
        //
        // 摘要:
        //     Returns the version of the underlying SQLite database engine
        public string ServerVersion {
            get
            {
                return _Connection.ServerVersion;
            }
        }
        //
        // 摘要:
        //     The extra connection flags to be used for all opened connections.
        public static SQLiteConnectionFlags SharedFlags {
            get
            {
                return SQLiteConnection.SharedFlags;
            }
            set
            {
                SQLiteConnection.SharedFlags = value;
            }
        }
        //
        // 摘要:
        //     Returns a string containing the compile-time options used to compile the
        //     SQLite core native library, delimited with spaces.
        public static string SQLiteCompileOptions
        {
            get
            {
                return SQLiteConnection.SQLiteCompileOptions;
            }
        }
        //
        // 摘要:
        //     This method returns the string whose value is the same as the SQLITE_SOURCE_ID
        //     C preprocessor macro used when compiling the SQLite core library.
        internal static string SQLiteSourceId
        {
            get
            {
                return SQLiteConnection.SQLiteSourceId;
            }
        }
        //
        // 摘要:
        //     Returns the version of the underlying SQLite core library.
        public static string SQLiteVersion
        {
            get
            {
                return SQLiteConnection.SQLiteVersion;
            }
        }
        //
        // 摘要:
        //     Returns the state of the connection.
        public ConnectionState State
        {
            get
            {
                return _Connection.State;
            }
        }

        // 摘要:
        //     Adds a per-connection type mapping, possibly replacing one or more that already
        //     exist.
        //
        // 参数:
        //   typeName:
        //     The case-insensitive database type name (e.g. "MYDATE"). The value of this
        //     parameter cannot be null. Using an empty string value (or a string value
        //     consisting entirely of whitespace) for this parameter is not recommended.
        //
        //   dataType:
        //     The System.Data.DbType value that should be associated with the specified
        //     type name.
        //
        //   primary:
        //     Non-zero if this mapping should be considered to be the primary one for the
        //     specified System.Data.DbType.
        //
        // 返回结果:
        //     A negative value if nothing was done. Zero if no per-connection type mappings
        //     were replaced (i.e. it was a pure add operation). More than zero if some
        //     per-connection type mappings were replaced.
        public int AddTypeMapping(string typeName, DbType dataType, bool primary)
        {
           return this._Connection.AddTypeMapping(typeName, dataType, primary);
        }
        //
        // 摘要:
        //     Backs up the database, using the specified database connection as the destination.
        //
        // 参数:
        //   destination:
        //     The destination database connection.
        //
        //   destinationName:
        //     The destination database name.
        //
        //   sourceName:
        //     The source database name.
        //
        //   pages:
        //     The number of pages to copy or negative to copy all remaining pages.
        //
        //   callback:
        //     The method to invoke between each step of the backup process. This parameter
        //     may be null (i.e. no callbacks will be performed).
        //
        //   retryMilliseconds:
        //     The number of milliseconds to sleep after encountering a locking error during
        //     the backup process. A value less than zero means that no sleep should be
        //     performed.
        internal void BackupDatabase(SQLiteConnBridge destination, string destinationName, string sourceName, int pages, SQLiteBackupCallback callback, int retryMilliseconds)
        {
            this._Connection.BackupDatabase(destination.Connection, destinationName, sourceName, pages, callback, retryMilliseconds);
        }
        //
        // 摘要:
        //     Creates a new System.Data.SQLite.SQLiteTransaction if one isn't already active
        //     on the connection.
        //
        // 返回结果:
        //     Returns the new transaction object.
        public SQLiteTransaction BeginTransaction()
        {
            return this._Connection.BeginTransaction();
        }
        //
        // 摘要:
        //     OBSOLETE. Creates a new SQLiteTransaction if one isn't already active on
        //     the connection.
        //
        // 参数:
        //   isolationLevel:
        //     This parameter is ignored.
        //
        //   deferredLock:
        //     When TRUE, SQLite defers obtaining a write lock until a write operation is
        //     requested.  When FALSE, a writelock is obtained immediately. The default
        //     is TRUE, but in a multi-threaded multi-writer environment, one may instead
        //     choose to lock the database immediately to avoid any possible writer deadlock.
        //
        // 返回结果:
        //     Returns a SQLiteTransaction object.
        public SQLiteTransaction BeginTransaction(IsolationLevel isolationLevel, bool deferredLock)
        {
            return this._Connection.BeginTransaction(isolationLevel, deferredLock);
        }
        //
        // 摘要:
        //     Attempts to bind the specified System.Data.SQLite.SQLiteFunction object instance
        //     to this connection.
        //
        // 参数:
        //   functionAttribute:
        //     The System.Data.SQLite.SQLiteFunctionAttribute object instance containing
        //     the metadata for the function to be bound.
        //
        //   function:
        //     The System.Data.SQLite.SQLiteFunction object instance that implements the
        //     function to be bound.
        public void BindFunction(SQLiteFunctionAttribute functionAttribute, SQLiteFunction function)
        {
            this._Connection.BindFunction(functionAttribute, function);
        }
        //
        // 摘要:
        //     This method causes any pending database operation to abort and return at
        //     its earliest opportunity. This routine is typically called in response to
        //     a user action such as pressing "Cancel" or Ctrl-C where the user wants a
        //     long query operation to halt immediately. It is safe to call this routine
        //     from any thread. However, it is not safe to call this routine with a database
        //     connection that is closed or might close before this method returns.
        public void Cancel()
        {
            this._Connection.Cancel();
        }
        //
        // 摘要:
        //     This method is not implemented; however, the System.Data.SQLite.SQLiteConnection.Changed
        //     event will still be raised.
        //
        // 参数:
        //   databaseName:
        internal override void ChangeDatabase(string databaseName)
        {
            this._Connection.ChangeDatabase(databaseName);
        }
        //
        // 摘要:
        //     Change the password (or assign a password) to an open database.
        //
        // 参数:
        //   newPassword:
        //     The new password to assign to the database
        //
        // 备注:
        //     No readers or writers may be active for this process. The database must already
        //     be open and if it already was password protected, the existing password must
        //     already have been supplied.
        internal void ChangePassword(byte[] newPassword)
        {
            this._Connection.ChangePassword(newPassword);
        }
        //
        // 摘要:
        //     Change the password (or assign a password) to an open database.
        //
        // 参数:
        //   newPassword:
        //     The new password to assign to the database
        //
        // 备注:
        //     No readers or writers may be active for this process. The database must already
        //     be open and if it already was password protected, the existing password must
        //     already have been supplied.
        internal void ChangePassword(string newPassword)
        {
            this._Connection.ChangePassword(newPassword);
        }
        //
        // 摘要:
        //     Clears all connection pools. Any active connections will be discarded instead
        //     of sent to the pool when they are closed.
        internal static void ClearAllPools()
        {
            SQLiteConnection.ClearAllPools();
        }
        //
        // 摘要:
        //     Clears the per-connection cached settings.
        //
        // 返回结果:
        //     The total number of per-connection settings cleared.
        internal int ClearCachedSettings()
        {
            return this._Connection.ClearCachedSettings();
        }
        //
        // 摘要:
        //     Clears the connection pool associated with the connection. Any other active
        //     connections using the same database file will be discarded instead of returned
        //     to the pool when they are closed.
        //
        // 参数:
        //   connection:
        internal static void ClearPool(SQLiteConnBridge connection)
        {
            SQLiteConnection.ClearPool(connection.Connection);
        }
        //
        // 摘要:
        //     Clears the per-connection type mappings.
        //
        // 返回结果:
        //     The total number of per-connection type mappings cleared.
        internal int ClearTypeMappings()
        {
            return this._Connection.ClearTypeMappings();
        }
        //
        // 摘要:
        //     Creates a clone of the connection. All attached databases and user-defined
        //     functions are cloned. If the existing connection is open, the cloned connection
        //     will also be opened.
        internal object Clone()
        {
            return new SQLiteConnBridge(this.Connection.Clone() as SQLiteConnection);
        }
        //
        // 摘要:
        //     When the database connection is closed, all commands linked to this connection
        //     are automatically reset.
        internal override void Close()
        {
            this._Connection.Close();
        }
        //
        // 摘要:
        //     Create a new System.Data.SQLite.SQLiteCommand and associate it with this
        //     connection.
        //
        // 返回结果:
        //     Returns a new command object already assigned to this connection.
        public SQLiteCommand CreateCommand()
        {
            return this._Connection.CreateCommand();
        }
        //
        // 摘要:
        //     Creates a database file. This just creates a zero-byte file which SQLite
        //     will turn into a database when the file is opened properly.
        //
        // 参数:
        //   databaseFileName:
        //     The file to create
        internal static void CreateFile(string databaseFileName)
        {
            SQLiteConnection.CreateFile(databaseFileName);
        }
        //
        // 摘要:
        //     Enables or disabled extension loading.
        //
        // 参数:
        //   enable:
        //     True to enable loading of extensions, false to disable.
        public void EnableExtensions(bool enable)
        {
            this._Connection.EnableExtensions(enable);
        }

        public SQLiteErrorCode ExtendedResultCode()
        {
            return this._Connection.ExtendedResultCode();
        }
        //
        // 摘要:
        //     Returns various global memory statistics for the SQLite core library via
        //     a dictionary of key/value pairs. Currently, only the "MemoryUsed" and "MemoryHighwater"
        //     keys are returned and they have values that correspond to the values that
        //     could be obtained via the System.Data.SQLite.SQLiteConnection.MemoryUsed
        //     and System.Data.SQLite.SQLiteConnection.MemoryHighwater connection properties.
        //
        // 参数:
        //   statistics:
        //     This dictionary will be populated with the global memory statistics. It will
        //     be created if necessary.
        public static void GetMemoryStatistics(ref IDictionary<string, long> statistics)
        {
            SQLiteConnection.GetMemoryStatistics(ref statistics);
        }
        //
        // 摘要:
        //     Returns the MetaDataCollections schema
        //
        // 返回结果:
        //     A DataTable of the MetaDataCollections schema
        public DataTable GetSchema()
        {
            return this._Connection.GetSchema();
        }
        //
        // 摘要:
        //     Returns schema information of the specified collection
        //
        // 参数:
        //   collectionName:
        //     The schema collection to retrieve
        //
        // 返回结果:
        //     A DataTable of the specified collection
        public DataTable GetSchema(string collectionName)
        {
            return this._Connection.GetSchema(collectionName);
        }
        //
        // 摘要:
        //     Retrieves schema information using the specified constraint(s) for the specified
        //     collection
        //
        // 参数:
        //   collectionName:
        //     The collection to retrieve
        //
        //   restrictionValues:
        //     The restrictions to impose
        //
        // 返回结果:
        //     A DataTable of the specified collection
        public DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            return this._Connection.GetSchema(collectionName, restrictionValues);
        }
        //
        // 摘要:
        //     Returns the per-connection type mappings.
        //
        // 返回结果:
        //     The per-connection type mappings -OR- null if they are unavailable.
        public Dictionary<string, object> GetTypeMappings()
        {
            return this._Connection.GetTypeMappings();
        }
        //
        // 摘要:
        //     Loads a SQLite extension library from the named dynamic link library file.
        //
        // 参数:
        //   fileName:
        //     The name of the dynamic link library file containing the extension.
        public void LoadExtension(string fileName)
        {
            this._Connection.LoadExtension(fileName);
        }
        //
        // 摘要:
        //     Loads a SQLite extension library from the named dynamic link library file.
        //
        // 参数:
        //   fileName:
        //     The name of the dynamic link library file containing the extension.
        //
        //   procName:
        //     The name of the exported function used to initialize the extension.  If null,
        //     the default "sqlite3_extension_init" will be used.
        public void LoadExtension(string fileName, string procName)
        {
            this._Connection.LoadExtension(fileName, procName);
        }
        //
        public void LogMessage(int iErrCode, string zMessage)
        {
            this._Connection.LogMessage(iErrCode, zMessage);
        }
        //
        public void LogMessage(SQLiteErrorCode iErrCode, string zMessage)
        {
            this._Connection.LogMessage(iErrCode, zMessage);
        }
        //
        // 摘要:
        //     Opens the connection using the parameters found in the System.Data.SQLite.SQLiteConnection.ConnectionString.
        internal void Open()
        {
            this._Connection.Open();
        }
        //
        // 摘要:
        //     Opens the connection using the parameters found in the System.Data.SQLite.SQLiteConnection.ConnectionString
        //     and then returns it.
        //
        // 返回结果:
        //     The current connection object.
        internal SQLiteConnBridge OpenAndReturn()
        {
            this._Connection.Open();
            return this;
        }
        //
        // 摘要:
        //     Attempts to free as much heap memory as possible for this database connection.
        internal void ReleaseMemory()
        {
            this._Connection.ReleaseMemory();
        }
        //
        // 摘要:
        //     Attempts to free N bytes of heap memory by deallocating non-essential memory
        //     allocations held by the database library. Memory used to cache database pages
        //     to improve performance is an example of non-essential memory. This is a no-op
        //     returning zero if the SQLite core library was not compiled with the compile-time
        //     option SQLITE_ENABLE_MEMORY_MANAGEMENT. Optionally, attempts to reset and/or
        //     compact the Win32 native heap, if applicable.
        //
        // 参数:
        //   nBytes:
        //     The requested number of bytes to free.
        //
        //   reset:
        //     Non-zero to attempt a heap reset.
        //
        //   compact:
        //     Non-zero to attempt heap compaction.
        //
        //   nFree:
        //     The number of bytes actually freed. This value may be zero.
        //
        //   resetOk:
        //     This value will be non-zero if the heap reset was successful.
        //
        //   nLargest:
        //     The size of the largest committed free block in the heap, in bytes.  This
        //     value will be zero unless heap compaction is enabled.
        //
        // 返回结果:
        //     A standard SQLite return code (i.e. zero for success and non-zero for failure).
        internal static SQLiteErrorCode ReleaseMemory(int nBytes, bool reset, bool compact, ref int nFree, ref bool resetOk, ref uint nLargest)
        {
            return SQLiteConnection.ReleaseMemory(nBytes, reset, compact, ref nFree, ref resetOk,ref nLargest);
        }
        //
        internal SQLiteErrorCode ResultCode()
        {
            return this._Connection.ResultCode();
        }
        //
        // 摘要:
        //     Queries or modifies the number of retries or the retry interval (in milliseconds)
        //     for certain I/O operations that may fail due to anti-virus software.
        //
        // 参数:
        //   count:
        //     The number of times to retry the I/O operation. A negative value will cause
        //     the current count to be queried and replace that negative value.
        //
        //   interval:
        //     The number of milliseconds to wait before retrying the I/O operation. This
        //     number is multiplied by the number of retry attempts so far to come up with
        //     the final number of milliseconds to wait. A negative value will cause the
        //     current interval to be queried and replace that negative value.
        //
        // 返回结果:
        //     Zero for success, non-zero for error.
        internal SQLiteErrorCode SetAvRetry(ref int count, ref int interval)
        {
            return this._Connection.SetAvRetry(ref count, ref interval);
        }
        //
        // 摘要:
        //     Sets the chunk size for the primary file associated with this database connection.
        //
        // 参数:
        //   size:
        //     The new chunk size for the main database, in bytes.
        //
        // 返回结果:
        //     Zero for success, non-zero for error.
        internal SQLiteErrorCode SetChunkSize(int size)
        {
            return this._Connection.SetChunkSize(size);
        }
        //
        internal void SetExtendedResultCodes(bool bOnOff)
        {
            this._Connection.SetExtendedResultCodes(bOnOff);
        }
        //
        // 摘要:
        //     Sets the status of the memory usage tracking subsystem in the SQLite core
        //     library. By default, this is enabled.  If this is disabled, memory usage
        //     tracking will not be performed. This is not really a per-connection value,
        //     it is global to the process.
        //
        // 参数:
        //   value:
        //     Non-zero to enable memory usage tracking, zero otherwise.
        //
        // 返回结果:
        //     A standard SQLite return code (i.e. zero for success and non-zero for failure).
        internal static SQLiteErrorCode SetMemoryStatus(bool value)
        {
            return SQLiteConnection.SetMemoryStatus(value);
        }
        //
        // 摘要:
        //     Sets the password for a password-protected database. A password-protected
        //     database is unusable for any operation until the password has been set.
        //
        // 参数:
        //   databasePassword:
        //     The password for the database
        internal void SetPassword(byte[] databasePassword)
        {
            this._Connection.SetPassword(databasePassword);
        }
        //
        // 摘要:
        //     Sets the password for a password-protected database. A password-protected
        //     database is unusable for any operation until the password has been set.
        //
        // 参数:
        //   databasePassword:
        //     The password for the database
        internal void SetPassword(string databasePassword)
        {
            this._Connection.SetPassword(databasePassword);
        }
        //
        // 摘要:
        //     Passes a shutdown request to the SQLite core library. Does not throw an exception
        //     if the shutdown request fails.
        //
        // 返回结果:
        //     A standard SQLite return code (i.e. zero for success and non-zero for failure).
        internal SQLiteErrorCode Shutdown()
        {
            return this._Connection.Shutdown();
        }
        //
        // 摘要:
        //     Passes a shutdown request to the SQLite core library. Throws an exception
        //     if the shutdown request fails and the no-throw parameter is non-zero.
        //
        // 参数:
        //   directories:
        //     Non-zero to reset the database and temporary directories to their default
        //     values, which should be null for both.
        //
        //   noThrow:
        //     When non-zero, throw an exception if the shutdown request fails.
        internal static void Shutdown(bool directories, bool noThrow)
        {
            SQLiteConnection.Shutdown(directories, noThrow);
        }
        internal void Dispose()
        {
            this._Connection.Dispose();
        }
        protected SQLiteConnection Connection
        {
            get
            {
                return _Connection;
            }
        }
        private SQLiteConnection _Connection;
    }
}
