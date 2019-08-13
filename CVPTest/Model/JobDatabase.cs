using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CVPTest.Model
{
    public class JobDatabase : DbContext
    {
        /// <summary>
        /// コンストラクタ
        /// 接続文字列が注入される
        /// </summary>
        /// <param name="options"></param>
        public JobDatabase(DbContextOptions<JobDatabase> options)
            : base(options)
        { }

        /// <summary>
        /// 設定情報上書き用
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { }

        /// <summary>
        /// 一部テーブルにUnique制約を設定したい
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // １グループに１ジョブのみの制約を課す
            modelBuilder.Entity<JobGroup>()
                .HasIndex(p => new { p.GroupId, p.JobId })
                .IsUnique();
        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobGroup> JobGroups { get; set; }
    }

    /// <summary>
    /// 基底。削除の代わりに Enabled で保持。
    /// </summary>
    public class EntityBase
    {
        public EntityBase()
        {
            Enabled = true;
            Modified = DateTime.UtcNow;
        }
        public bool Enabled { get; set; }
        public DateTime? Modified { get; set; }
    }

    /// <summary>
    /// アップロードされたファイルのメタデータを保持
    /// </summary>
    public class Job : EntityBase
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// アップロードする時のファイル名
        /// </summary>
        public string LogicalName { get; set; }
        /// <summary>
        /// Blob保存時のファイル名
        /// </summary>
        public string PhysicalName { get; set; }
        /// <summary>
        /// BlobへのURL
        /// </summary>
        public string PhysicalPath { get; set; }
    }

    /// <summary>
    /// 複数のジョブをグルーピング
    /// </summary>
    public class JobGroup : EntityBase
    {
        [Key]
        public int Id { get; set; }
        public Guid GroupId { get; set; }

        public int JobId { get; set; }
        [ForeignKey("JobId")]
        public Job Job { get; set; }
    }
}
