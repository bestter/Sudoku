// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


var easyPuzzleStr = "..6..73..\n.18..9.5.\n5......64\n92..8....\n...763...\n....9..75\n63......8\n.9.3..52.\n..24..6.."

function main() {
    enableConsole();
    var sudoku = new Sudoku();
    sudoku.importPuzzle(easyPuzzleStr);
    var generator = new SudokuGenerator()
    var solver = new SudokuSolver();

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

function Sudoku() {
    var $domobj = $("#sudoku");
    var $table = $("<table>").addClass("sudokuTable");
    var _self = this;
    var selectedCell = new Vec2d();
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

            data[y][x] = new Cell(new Vec2d(x, y), numBox, this);
        }
    }

    $(document).on("keydown", function (e) {
        var evt = e || window.event;

        // Prevent backspace from navigating back.
        if (e.which === 8 && !$(e.target).is("input, textarea"))
            e.preventDefault();

        switch (evt.which) {
            case 37: // left
                _self.moveSelected(-1, 0);
                break;

            case 38: // up
                _self.moveSelected(0, -1);
                break;

            case 39: // right
                _self.moveSelected(1, 0);
                break;

            case 40: // down
                _self.moveSelected(0, 1);
                break;

            case 8: // backspace
            case 46: // delete
            case 48: // zero
                _self.clearSelectedValue();
                break;

            case 49:
            case 50:
            case 51:
            case 52:
            case 53:
            case 54:
            case 55:
            case 56:
            case 57: // 1-9
                _self.setSelectedValue(evt.keyCode - 48);
                break;
        }
    });

    function getBoxNum(x, y) {
        var boxX = Math.floor(x / 3);
        var boxY = Math.floor(y / 3);
        return boxY * 3 + boxX;
    }

    function boxNumToCoords(num) {
        var boxX = num % 3;
        var boxY = Math.floor(num / 3);
        return new Vec2d(boxX * 3, boxY * 3);
    }

    function checkRowForErrors(row) {
        var nums = [];
        var error = false;

        for (var col = 0; col < 9; col++) {
            var curVal = data[row][col].value;
            if (curVal !== '' && nums.indexOf(curVal) !== -1) {
                error = true;
                break;
            } else
                nums.push(curVal);
        }

        if (error)
            for (var col = 0; col < 9; col++)
                data[row][col].addError();

        return error;
    }

    function checkColForErrors(col) {
        var nums = [];
        var error = false;

        for (var row = 0; row < 9; row++) {
            var curVal = data[row][col].value;
            if (curVal !== '' && nums.indexOf(curVal) !== -1) {
                error = true;
                break;
            } else
                nums.push(curVal);
        }

        if (error)
            for (var row = 0; row < 9; row++)
                data[row][col].addError();

        return error;
    }

    function checkBoxForErrors(box) {
        var nums = [];
        var error = false;
        var boxCoords = boxNumToCoords(box);

        for (var row = boxCoords.y; row < boxCoords.y + 3; row++) {
            for (var col = boxCoords.x; col < boxCoords.x + 3; col++) {
                var curVal = data[row][col].value;
                if (curVal !== '' && nums.indexOf(curVal) !== -1) {
                    error = true;
                    break;
                } else
                    nums.push(curVal);
            }
        }

        if (error)
            for (var row = boxCoords.y; row < boxCoords.y + 3; row++)
                for (var col = boxCoords.x; col < boxCoords.x + 3; col++)
                    data[row][col].addError();

        return error;
    }

    function checkBoardForErrors() {
        clearBoardErrors();
        for (var i = 0; i < 9; i++) {
            checkRowForErrors(i);
            checkColForErrors(i);
            checkBoxForErrors(i);
        }
    }

    function clearBoardErrors() {
        for (var row = 0; row < 9; row++)
            for (var col = 0; col < 9; col++)
                data[row][col].removeError();
    }

    function checkSelectedForErrors() {
        clearBoardErrors();
        checkRowForErrors(selectedRow);
        checkColForErrors(selectedCol);
        checkBoxForErrors(selectedBox);
    }

    function deselectRow(row) {
        for (var x = 0; x < data[row].length; x++)
            data[row][x].deselect();
    }

    function deselectColumn(col) {
        for (var y = 0; y < data.length; y++)
            data[y][col].deselect();
    }

    function deselectBox(box) {
        var boxCoords = boxNumToCoords(box);
        for (var y = boxCoords.y; y < boxCoords.y + 3; y++)
            for (var x = boxCoords.x; x < boxCoords.x + 3; x++)
                data[y][x].deselect();
    }

    function deselectNum(num) {
        for (var y = 0; y < 9; y++)
            for (var x = 0; x < 9; x++)
                if (data[y][x].value === num && num !== '')
                    data[y][x].deselect();
    }

    function selectRow(row) {
        for (var x = 0; x < data[row].length; x++)
            data[row][x].secondarySelect();
    }

    function selectColumn(col) {
        for (var y = 0; y < data.length; y++)
            data[y][col].secondarySelect();
    }

    function selectBox(box) {
        var boxCoords = boxNumToCoords(box);
        for (var y = boxCoords.y; y < boxCoords.y + 3; y++)
            for (var x = boxCoords.x; x < boxCoords.x + 3; x++)
                data[y][x].secondarySelect();
    }

    function selectNum(num) {
        for (var y = 0; y < 9; y++)
            for (var x = 0; x < 9; x++)
                if (data[y][x].value === num && num !== '')
                    data[y][x].sameNumberSelect();
    }

    this.setSelected = function (xPos, yPos) {
        // deselect all row, column, box
        deselectRow(selectedRow);
        deselectColumn(selectedCol);
        deselectBox(selectedBox);
        deselectNum(selectedNum);

        // update selected
        selectedRow = yPos;
        selectedCol = xPos;
        selectedBox = getBoxNum(xPos, yPos);
        selectedNum = data[yPos][xPos].value;
        selectedCell.set(xPos, yPos);

        // reselect
        selectRow(selectedRow);
        selectColumn(selectedCol);
        selectBox(selectedBox);
        selectNum(selectedNum);
        data[yPos][xPos].select();
    };

    this.moveSelected = function (xOffset, yOffset) {
        var newX = (selectedCell.x + xOffset) % 9;
        while (newX < 0) newX += 9;
        var newY = (selectedCell.y + yOffset) % 9;
        while (newY < 0) newY += 9;
        this.setSelected(newX, newY);
    };

    this.setSelectedValue = function (val) {
        if (data[selectedCell.y][selectedCell.x].setValue(val)) {
            deselectNum(selectedNum);
            selectedNum = val;
            selectNum(selectedNum);
            data[selectedCell.y][selectedCell.x].select();
            checkBoardForErrors();
        }
    };

    this.clearSelectedValue = function () {
        if (data[selectedCell.y][selectedCell.x].clearValue()) {
            deselectNum(selectedNum);
            selectedNum = '';
            checkBoardForErrors();
        }
    };

    this.importPuzzle = function (puzzleStr) {
        var inProgress = false;

        if (puzzleStr.indexOf('[') !== -1)
            inProgress = true;
        if (puzzleStr === '') {
            console.log("ERROR: Invalid Puzzle Given.");
            return;
        }

        var curRow = 0;
        var curCol = 0;
        var nextIsLocked = false;

        for (var i = 0; i < puzzleStr.length; i++) {
            curChar = puzzleStr[i];
            if (curChar >= 1 && curChar <= 9) // is digit 1-9
            {
                if (curRow > 8 || curCol > 8) {
                    console.log("ERROR: invalid puzzle matrix");
                    return;
                }
                var num = curChar;
                data[curRow][curCol].setValue(parseInt(num));
                if (!inProgress || nextIsLocked)
                    data[curRow][curCol].setLocked(true);
                else
                    data[curRow][curCol].setLocked(false);
                curCol++;
                nextIsLocked = false;
            } else if (curChar === '.') {
                if (curRow > 8 || curCol > 8) {
                    console.log("ERROR: invalid puzzle matrix");
                    return;
                }
                data[curRow][curCol].setValue('');
                data[curRow][curCol].setLocked(false);
                nextIsLocked = false;
                curCol++;
            } else if (curChar === '[') {
                nextIsLocked = true;
            } else if (curChar === '\n') {
                if (curCol != 9) {
                    console.log("ERROR: invalid puzzle matrix");
                    return;
                }
                curCol = 0;
                curRow++;
                nextIsLocked = false;
            }
        }

        checkBoardForErrors();
    };

    this.clearUserInput = function () {
        clearBoardErrors();
        for (var row = 0; row < data.length; row++)
            for (var col = 0; col < data[row].length; col++)
                if (!data[row][col].locked)
                    data[row][col].clearValue();
        checkBoardForErrors();
        this.setSelected(selectedCol, selectedRow);
    };

    this.clearBoard = function () {
        for (var row = 0; row < data.length; row++)
            for (var col = 0; col < data[row].length; col++) {
                data[row][col].setLocked(false);
                data[row][col].clearValue();
            }
        clearBoardErrors();
        this.setSelected(selectedCol, selectedRow);
    };

    this.getPuzzleArrayStr = function () {
        //* Print board values 2d array where columns are num, row, col
        var txt = "";
        txt += "[";
        for (var i = 0; i < data.length; i++)
            for (var j = 0; j < data[i].length; j++)
                if (data[i][j].value !== '')
                    txt += ("[" + data[i][j].value + ", " + i + ", " + j + "],");
        txt += "];";

        return txt;
    };

    this.setSelected(0, 0);
}



function Cell(pos, domobj, parent, locked, value) {
    this.value = typeof value !== 'undefined' ? value : '';
    this.locked = typeof locked !== 'undefined' ? locked : false;
    var parent = parent;
    var $domobj = domobj;
    $domobj.append($("<div>"));
    var $number = $($domobj.find("div")[0]);
    var pos = pos;
    var _self = this;

    $domobj.click(function () {
        parent.setSelected(pos.x, pos.y);
    });
    this.deselect = function () {
        $domobj.removeClass("selectedCell secondarySelected sameNumber");
    };
    this.select = function () {
        $domobj.removeClass("secondarySelected sameNumber").addClass("selectedCell");
    };
    this.secondarySelect = function () {
        $domobj.addClass("secondarySelected");
    };
    this.sameNumberSelect = function () {
        $domobj.addClass("sameNumber");
    };
    this.setValue = function (val) {
        if (val >= 1 && val <= 9 && this.locked === false) {
            this.value = val;
            $number.text(this.value);
            return true;
        }
        return false;
    };
    this.setLocked = function (locked) {
        this.locked = locked;
        if (locked)
            $domobj.addClass("locked");
        else
            $domobj.removeClass("locked");
    };
    this.setLocked(this.locked);
    this.clearValue = function () {
        if (this.locked === false) {
            this.value = '';
            $number.text(this.value);
            return true;
        }
        return false;
    };
    this.addError = function () {
        $domobj.addClass("error");
    }
    this.removeError = function () {
        $domobj.removeClass("error");
    }

    this.setValue(this.value);
}



function SudokuGenerator() {

}



function SudokuSolver() {

}



/********** Vec2d Class **********/
function Vec2d(newX, newY) {
    this.x = typeof newX !== 'undefined' ? newX : 0;
    this.y = typeof newY !== 'undefined' ? newY : 0;
}
/********** Vec2d Methods **********/
Vec2d.prototype.valueOf = function () {
    return {
        x: this.x,
        y: this.y
    };
};
Vec2d.prototype.getOffset = function (x, y) {
    return new Vec2d(this.x + x, this.y + y);
};
Vec2d.prototype.set = function (x, y) {
    this.x = x;
    this.y = y;
};
/********** Vec2d Static Methods **********/
Vec2d.add = function (v1, v2) {
    return new Vec2d(v1.x + v2.x, v1.y + v2.y);
};
