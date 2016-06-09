﻿using System.Collections.Generic;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class SkillList : GameServerNetworkPacket
    {
        private readonly IList<TSkill> skills;
        private readonly int _blockAct;
        private readonly int _blockSpell;
        private readonly int _blockSkill;

        public SkillList(L2Player player, int blockAct, int blockSpell, int blockSkill)
        {
            skills = player._skills.Values;
            _blockAct = blockAct;
            _blockSpell = blockSpell;
            _blockSkill = blockSkill;
        }

        protected internal override void write()
        {
            writeC(0x58);
            writeD(skills.Count);

            foreach (TSkill skill in skills)
            {
                int passive = skill.isPassive();
                writeD(passive);
                writeD(skill.level);
                writeD(skill.skill_id);

                byte blocked = 0;
                if (passive == 0)
                    if (_blockAct == 1)
                        blocked = 1;
                    else
                        switch (skill.is_magic)
                        {
                            case 0:
                                blocked = (byte)_blockSkill;
                                break;
                            case 1:
                                blocked = (byte)_blockSpell;
                                break;
                            default:
                                blocked = 0;
                                break;
                        }

                writeC(blocked);
            }
        }
    }
}