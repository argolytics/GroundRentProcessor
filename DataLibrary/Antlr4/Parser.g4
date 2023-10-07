chat                : line+ EOF ;
line                : name command message NEWLINE;
message             : (emoticon | link | color | mention | WORD | WHITESPACE)+ ;
name                : WORD WHITESPACE;
command             : (SAYS | SHOUTS) ':' WHITESPACE ;                            
emoticon            : ':' '-'? ')'
                    | ':' '-'? '('
                    ;
link                : '[' TEXT ']' '(' TEXT ')' ;
color               : '/' WORD '/' message '/';
mention             : '@' WORD ;