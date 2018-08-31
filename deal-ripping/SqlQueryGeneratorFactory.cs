
namespace Trinity.DealRipping
{
	public class SqlQueryGeneratorFactory
	{
		public static ISqlQueryGenerator GetSqlQueryGenerator (PrincipalProtectedNoteContainer container)
		{
			var sqlQueryGenerator = new PrincipalProtectedNoteSqlQueryGenerator {Container = container};
			return sqlQueryGenerator;
		}

		public static ISqlQueryGenerator GetSqlQueryGenerator (PLUSContainer container)
		{
			PlusSqlQueryGenerator sqlQueryGenerator = new PlusSqlQueryGenerator ();
			sqlQueryGenerator.Container = container;
			return sqlQueryGenerator;
		}

		public static ISqlQueryGenerator GetSqlQueryGenerator (BufferedPLUSContainer container)
		{
			BufferedPLUSSqlQueryGenerator sqlQueryGenerator = new BufferedPLUSSqlQueryGenerator ();
			sqlQueryGenerator.Container = container;
			return sqlQueryGenerator;
		}

		public static ISqlQueryGenerator GetSqlQueryGenerator (TriggerPLUSContainer container)
		{
			TriggerPlusSqlQueryGenerator sqlQueryGenerator = new TriggerPlusSqlQueryGenerator ();
			sqlQueryGenerator.Container = container;
			return sqlQueryGenerator;
		}

		public static ISqlQueryGenerator GetSqlQueryGenerator (DualDirectionalTriggerPLUSContainer container)
		{
			DualDirectionalTriggerPlusSqlQueryGenerator sqlQueryGenerator = new DualDirectionalTriggerPlusSqlQueryGenerator ();
			sqlQueryGenerator.Container = container;
			return sqlQueryGenerator;
		}

		public static ISqlQueryGenerator GetSqlQueryGenerator (JumpContainer container)
		{
			JumpSqlQueryGenerator sqlQueryGenerator = new JumpSqlQueryGenerator ();
			sqlQueryGenerator.Container = container;
			return sqlQueryGenerator;
		}

		public static ISqlQueryGenerator GetSqlQueryGenerator (BufferedJumpContainer container)
		{
			BufferedJumpSqlQueryGenerator sqlQueryGenerator = new BufferedJumpSqlQueryGenerator ();
			sqlQueryGenerator.Container = container;
			return sqlQueryGenerator;
		}

		public static ISqlQueryGenerator GetSqlQueryGenerator (EnhancedTriggerJumpContainer container)
		{
			TriggerJumpSqlQueryGenerator sqlQueryGenerator = new TriggerJumpSqlQueryGenerator ();
			sqlQueryGenerator.Container = container;
			return sqlQueryGenerator;
		}

		public static ISqlQueryGenerator GetSqlQueryGenerator (TriggerJumpContainer container)
		{
			TriggerJumpSqlQueryGenerator sqlQueryGenerator = new TriggerJumpSqlQueryGenerator ();
			sqlQueryGenerator.Container = container;
			return sqlQueryGenerator;
		}

		public static ISqlQueryGenerator GetSqlQueryGenerator (DualDirectionalTriggerJumpContainer container)
		{
			DualDirectionalTriggerJumpSqlQueryGenerator sqlQueryGenerator = new DualDirectionalTriggerJumpSqlQueryGenerator ();
			sqlQueryGenerator.Container = container;
			return sqlQueryGenerator;
		}

		public static ISqlQueryGenerator GetSqlQueryGenerator (AutocallableContainer container)
		{
			AutocallableSqlQueryGenerator sqlQueryGenerator = new AutocallableSqlQueryGenerator ();
			sqlQueryGenerator.Container = container;
			return sqlQueryGenerator;
		}

		public static ISqlQueryGenerator GetSqlQueryGenerator (StepUpAutocallableContainer container)
		{
			StepUpAutocallableSqlQueryGenerator sqlQueryGenerator = new StepUpAutocallableSqlQueryGenerator ();
			sqlQueryGenerator.Container = container;
			return sqlQueryGenerator;
		}

		public static ISqlQueryGenerator GetSqlQueryGenerator (StepDownAutocallableContainer container)
		{
			StepDownAutocallableSqlQueryGenerator sqlQueryGenerator = new StepDownAutocallableSqlQueryGenerator ();
			sqlQueryGenerator.Container = container;
			return sqlQueryGenerator;
		}
	}
}