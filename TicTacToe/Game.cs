using System.Collections.Immutable;
using System.Text;
using Microsoft.AspNetCore.Html;

namespace TicTacToe;

public enum Symbol
{
  X,
  O,
  Empty
}

public class Game
{
  public static readonly Game Empty = new(ImmutableArray.Create(
    Symbol.Empty, Symbol.Empty, Symbol.Empty,
    Symbol.Empty, Symbol.Empty, Symbol.Empty,
    Symbol.Empty, Symbol.Empty, Symbol.Empty),
    Symbol.X);

  private readonly ImmutableArray<Symbol> squares;
  private readonly Symbol nextPlayer;

  private Game(ImmutableArray<Symbol> squares, Symbol nextPlayer)
  {
    this.squares = squares;
    this.nextPlayer = nextPlayer;
  }

  public Symbol NextPlayer => nextPlayer;

  public IEnumerable<int> EmptySquares
  {
    get
    {
      for (int i = 0; i < squares.Length; i++)
      {
        if (squares[i] == Symbol.Empty)
        {
          yield return i;
        }
      }
    }
  }

  public Game Play(int index)
  {
    if (squares[index] != Symbol.Empty)
    {
      throw new InvalidOperationException("Square is not empty");
    }

    var newSquares = squares.SetItem(index, nextPlayer);
    return new Game(newSquares, nextPlayer == Symbol.X ? Symbol.O : Symbol.X);
  }

  public Symbol Winner
  {
    get
    {
      if (squares[0] != Symbol.Empty && squares[0] == squares[1] && squares[1] == squares[2])
      {
        return squares[0];
      }
      if (squares[3] != Symbol.Empty && squares[3] == squares[4] && squares[4] == squares[5])
      {
        return squares[3];
      }
      if (squares[6] != Symbol.Empty && squares[6] == squares[7] && squares[7] == squares[8])
      {
        return squares[6];
      }
      if (squares[0] != Symbol.Empty && squares[0] == squares[3] && squares[3] == squares[6])
      {
        return squares[0];
      }
      if (squares[1] != Symbol.Empty && squares[1] == squares[4] && squares[4] == squares[7])
      {
        return squares[1];
      }
      if (squares[2] != Symbol.Empty && squares[2] == squares[5] && squares[5] == squares[8])
      {
        return squares[2];
      }
      if (squares[0] != Symbol.Empty && squares[0] == squares[4] && squares[4] == squares[8])
      {
        return squares[0];
      }
      if (squares[2] != Symbol.Empty && squares[2] == squares[4] && squares[4] == squares[6])
      {
        return squares[2];
      }
      return Symbol.Empty;
    }
  }

  public HtmlString Html => GenerateHtml();
  public HtmlString HtmlWithOutcome(Symbol outcome) => GenerateHtml(outcome);

  private HtmlString GenerateHtml(Symbol? outcome = null)
  {
    var sb = new StringBuilder();
    sb.Append("""
    <!DOCTYPE html>
    <html>
    <head>
      <title>Tic-Tac-Toe Board</title>
      <style>
        .board {
          display: flex;
          flex-direction: column;
        }

        .row {
          display: flex;
        }

        .cell {
          width: 50px;
          height: 50px;
          border: 1px solid black;
          font-size: 40px;
          display: flex;
          justify-content: center;
          align-items: center;
        }
      </style>
    </head>
    <body>
      <div class="board">
    """);

    for (int i = 0; i < 3; i++)
    {
      sb.Append("<div class=\"row\">");
      for (int j = 0; j < 3; j++)
      {
        sb.Append("<div class=\"cell\">");
        var index = i * 3 + j;
        sb.Append(squares[index] switch
        {
          Symbol.X => "X",
          Symbol.O => "O",
          _ => ""
        });
        sb.Append("</div>");
      }
      sb.Append("</div>");
    }
    if (outcome != null)
    {
      sb.Append("<div>");
      sb.Append(outcome switch
      {
        Symbol.X => "X wins",
        Symbol.O => "O wins",
        _ => "Draw"
      });
      sb.Append("</div>");
    }
    sb.Append("""
      </div>
    </body>
    </html>
    """);
    return new HtmlString(sb.ToString());
  }
}
