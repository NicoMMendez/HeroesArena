﻿using System.Collections.Generic;

namespace HeroesArena
{
    // Represents a one cell move of any unit.
    public class MoveAction : Action
    {
        // Checks if position is in action range.
        public override bool InRange(Coordinates pos, Map map)
        {
            if (map ==null || map.Cells == null || !map.Cells.ContainsKey(pos) || map.Cells[pos] == null || map.Cells[pos].Tile == null || !map.Cells[pos].Tile.Walkable)
                return false;
            BasicUnit unit = Executer as BasicUnit;
            int distance = unit.Cell.Position.Distance(pos);
            if (distance != 1)
                return false;
            return true;
        }

        // Move execution.
        public override bool Execute(List<Cell> targets, Map map)
        {
            // If no target.
            if (targets == null)
                return false;

            // If more than one target.
            if (targets.Count != 1)
                return false;

            Cell cell = targets[0];

            // If no executer or cell to move to.
            if (Executer == null || cell == null)
                return false;
            
            BasicUnit unit = Executer as BasicUnit;

            // If cell is not in range.
            if (!InRange(cell, map))
                return false;

            // If there is no place to move to.
            if (!cell.Tile.Walkable || cell.Unit != null)
                return false;

            // If not enough action points.
            if (!unit.UseActionPoints(Cost))
                return false;

            unit.UpdateFacing(cell);

            unit.Cell.Unit = null;
            unit.Cell = cell;
            cell.Unit = unit;
            
            return true;
        }

        // Constructor.
        public MoveAction(BasicUnit executer, int cost = 0) : base(executer, cost)
        {
            Tag = ActionTag.Move;
        }
    }
}