grammar ST;

/*
 * Parser Rules
 */

compileUnit
	:	programDeclaration | functionDeclaration | functionBlockDeclaration;

programDeclaration : 'PROGRAM' ID varDeclarationList statementList 'END_PROGRAM';
functionBlockDeclaration : 'FUNCTION_BLOCK' ID (':' ID)? varDeclarationList statementList 'END_FUNCTION_BLOCK';
functionDeclaration : 'FUNCTION' ID (':' ID)? varDeclarationList statementList 'END_FUNCTION';

varDeclarationList: varDeclarationSet*;
varDeclarationSet: 'VAR' (varDeclaration)* 'END_VAR' |
					'VAR_INPUT' (varDeclaration)* 'END_VAR' |
					'VAR_OUTPUT' (varDeclaration)* 'END_VAR' ;

varDeclaration: ID (','ID)* ':' ID';'| ID ':' ID ':=' expression';';

expression			: operand | expression OPERATOR expression | '(' expression ')' ;
operand				: NUMERIC | STRING | identifier | functioncall;

/*
 * Statements
 */

statementList : (statement)*;
statementAndBreakList : (statement| break)*;
statement: repeatLoop ';'| assignment';' | ifthen';' | functioncall';' | case';' | forLoop';' | whileLoop';';

assignment: identifier ':=' expression;

repeatLoop: 'REPEAT' statementAndBreakList 'UNTIL' expression 'END_REPEAT';
whileLoop: 'WHILE' expression 'DO' statementAndBreakList 'END_WHILE';
break: 'BREAK' ';';

ifthen: 'IF' expression 'THEN' statementList 'END_IF'|
		'IF' expression 'THEN' statementList ('ELSEIF' expression 'THEN' statementList)* ('ELSE' statementList)? 'END_IF';

forLoop : 'FOR' ID ':=' NUMERIC 'TO' NUMERIC ('BY' NUMERIC)? 'DO' statementAndBreakList 'END_FOR';

identifier : ID | identifier '.' ID;

functioncall		: identifier '(' calllist ')';
callitem			: operand (':=' expression )? (',')?;
calllist			: callitem*;

case				: 'CASE' identifier 'OF' caseItemList ('ELSE' statementList)? 'END_CASE';
caseItemList		: caseItem*;
caseItem			: operand':' statementList;



/*
 * Lexer Rules
 */
ID					: ('_' | ('a'..'z') | ('A'..'Z'))('_' | ('a'..'z') | ('A'..'Z') |('0'..'9'))*;

NUMERIC				: INTEGER |FLOAT;


WHITESPACE          : (' '|'\t')+ -> skip ;
NEWLINE             : ('\r'?'\n' | '\r')+ ->skip ;
OPERATOR			: '+' |'-' | '*' |'/'| '>' | '<' | '=' | '>=' | '<=';
INTEGER				: ('0'..'9')+;
STRING			    : '\'' ('\'\'' | ~ ('\''))* '\'';
FLOAT			    : ('0' .. '9') + (('.' ('0' .. '9') + (EXPONENT)?)? | EXPONENT);
fragment EXPONENT   : ('e') ('+' | '-')? ('0' .. '9') +;


/* Comments */
SingleLineComment
 : '//' ~('\r' | '\n')*->Skip
 ;

MultiLineComment
 : '/*' .* '*/' -> Skip
 ;
