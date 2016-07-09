﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using TSQL;
using TSQL.Statements;

using Tests.Tokens;

namespace Tests.Statements
{
	[TestFixture(Category = "Statement Parsing")]
	public class SelectStatementTests
	{
		[Test]
		public void SelectStatement_SelectLiteral()
		{
			List<TSQLStatement> statements = TSQLStatementReader.ParseStatements(
				"select 1;",
				includeWhitespace : true);
			Assert.IsNotNull(statements);
			Assert.AreEqual(1, statements.Count);
			Assert.AreEqual(TSQLStatementType.Select, statements[0].Type);
			Assert.AreEqual(4, statements[0].Tokens.Count);
			Assert.AreEqual(TSQLKeywords.SELECT, statements[0].Tokens[0].AsKeyword.Keyword);
			Assert.AreEqual(" ", statements[0].Tokens[1].AsWhitespace.Text);
			Assert.AreEqual("1", statements[0].Tokens[2].AsNumericLiteral.Text);
			Assert.AreEqual(TSQLCharacters.Semicolon, statements[0].Tokens[3].AsCharacter.Character);
		}

		[Test]
		public void SelectStatement_TwoLiteralSelects()
		{
			List<TSQLStatement> statements = TSQLStatementReader.ParseStatements(
				"select 1;select 2;",
				includeWhitespace: true);
			Assert.IsNotNull(statements);
			Assert.AreEqual(2, statements.Count);
			Assert.AreEqual(TSQLStatementType.Select, statements[0].Type);
			Assert.AreEqual(4, statements[0].Tokens.Count);
			Assert.AreEqual(TSQLKeywords.SELECT, statements[0].Tokens[0].AsKeyword.Keyword);
			Assert.AreEqual(" ", statements[0].Tokens[1].AsWhitespace.Text);
			Assert.AreEqual("1", statements[0].Tokens[2].AsNumericLiteral.Text);
			Assert.AreEqual(TSQLCharacters.Semicolon, statements[0].Tokens[3].AsCharacter.Character);
			Assert.AreEqual(TSQLStatementType.Select, statements[1].Type);
			Assert.AreEqual(4, statements[1].Tokens.Count);
			Assert.AreEqual(TSQLKeywords.SELECT, statements[1].Tokens[0].AsKeyword.Keyword);
			Assert.AreEqual(" ", statements[1].Tokens[1].AsWhitespace.Text);
			Assert.AreEqual("2", statements[1].Tokens[2].AsNumericLiteral.Text);
			Assert.AreEqual(TSQLCharacters.Semicolon, statements[1].Tokens[3].AsCharacter.Character);
		}

		[Test]
		public void SelectStatement_CorrelatedSelect()
		{
			List<TSQLStatement> statements = TSQLStatementReader.ParseStatements(
				"select (select 1);",
				includeWhitespace: true);
			Assert.IsNotNull(statements);
			Assert.AreEqual(1, statements.Count);
			Assert.AreEqual(TSQLStatementType.Select, statements[0].Type);
			Assert.AreEqual(8, statements[0].Tokens.Count);
			Assert.AreEqual(TSQLKeywords.SELECT, statements[0].Tokens[0].AsKeyword.Keyword);
			Assert.AreEqual(" ", statements[0].Tokens[1].AsWhitespace.Text);
			Assert.AreEqual("(", statements[0].Tokens[2].AsCharacter.Text);
			Assert.AreEqual("select", statements[0].Tokens[3].AsKeyword.Text);
			Assert.AreEqual(" ", statements[0].Tokens[4].AsWhitespace.Text);
			Assert.AreEqual("1", statements[0].Tokens[5].AsNumericLiteral.Text);
			Assert.AreEqual(")", statements[0].Tokens[6].AsCharacter.Text);
			Assert.AreEqual(";", statements[0].Tokens[7].AsCharacter.Text);
		}

		[Test]
		public void SelectStatement_CommonSelect()
		{
			List<TSQLStatement> statements = TSQLStatementReader.ParseStatements(
				@"select t.a, t.b, (select 1) as e
				from
					[table] t
						inner join [table] t2 on
							t.id = t2.id
				where
					t.c = 5
				group by
					t.a,
					t.b
				having
					count(*) > 1
				order by
					t.a,
					t.b;",
				includeWhitespace: true);
			Assert.IsNotNull(statements);
			Assert.AreEqual(1, statements.Count);
			Assert.AreEqual(TSQLStatementType.Select, statements[0].Type);
			Assert.AreEqual(95, statements[0].Tokens.Count);
			Assert.AreEqual(TSQLKeywords.SELECT, statements[0].Tokens[0].AsKeyword.Keyword);
			Assert.AreEqual(" ", statements[0].Tokens[1].AsWhitespace.Text);
			Assert.AreEqual("t", statements[0].Tokens[2].AsIdentifier.Name);
			Assert.AreEqual(TSQLCharacters.Period, statements[0].Tokens[3].AsCharacter.Character);
		}
	}
}
