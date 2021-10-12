// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var easyPuzzleStr = "..6..73..\n.18..9.5.\n5......64\n92..8....\n...763...\n....9..75\n63......8\n.9.3..52.\n..24..6.."

function main() {
    //enableConsole();
    //var sudoku = new Sudoku();
    //sudoku.importPuzzle(easyPuzzleStr);
    //var generator = new SudokuGenerator()
    //var solver = new SudokuSolver();

    $("#export_btn").click(function () {
        $("#output").text(sudoku.getPuzzleArrayStr());
    });

    $("#reset_btn").click(function () {
        sudoku.clearUserInput();
        console.log("Puzzle Reset!");
    });

    $("#clear_btn").click(function () {
        sudoku.clearBoard();
        console.log("Board Cleared!");
    });
}

$(document).ready(main);

/*
function Sudoku() {

    var $domobj = $("#sudoku");
    var $table = $("<table>").addClass("sudokuTable");
    var _self = this;
    var selectedCell = new Cell();
    var selectedRow = 0;
    var selectedCol = 0;
    var selectedBox = 0;
    var selectedNum = '';

    $domobj.append($table);

    var data = new Array(9);
    for (var y = 0; y < data.length; y++) {
        var curRow = $("<tr>");
        $table.append(curRow);
        data[y] = new Array(9);

        for (var x = 0; x < data[y].length; x++) {
            var cell = $("<td>");
            cell.addClass("sudokuCell");

            if (x % 3 === 2 && x !== 8)
                cell.addClass("hspace");
            if (y % 3 === 2 && y !== 8)
                cell.addClass("vspace");

            curRow.append(cell);
            var numBox = $("<div>").addClass("sudokuNumberBox");
            cell.append(numBox);

            data[y][x] = new Cell(new SCell(x, y), numBox, this);
        }
    }

    this.importPuzzle = function (puzzleStr) {

    };
}


function Cell(pos, domobj, parent, value) {
    var pos = pos;
    var $domobj = domobj;
    $domobj.append($("<div>"));
    var parent = parent;
    var value = value;
    var _self = this;
}

function SCell(newX, newY) {
    this.x = typeof newX !== 'undefined' ? newX : 0;
    this.y = typeof newY !== 'undefined' ? newY : 0;
}
SCell.prototype.valueOf = function () {
    return {
        x: this.x,
        y: this.y
    };
};
SCell.prototype.getOffset = function (x, y) {
    return new Vec2d(this.x + x, this.y + y);
};
SCell.prototype.set = function (x, y) {
    this.x = x;
    this.y = y;
};
SCell.add = function (v1, v2) {
    return new Vec2d(v1.x + v2.x, v1.y + v2.y);
};

        };
        */