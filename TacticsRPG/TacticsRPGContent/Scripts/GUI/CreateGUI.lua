local m_mainList
local m_actionList
local m_abilityList

function OnLoad()
	--[[
	m_mainList = { next = m_mainList, value = "Move" }
	m_mainList = { next = m_mainList, value = "Action" }
	m_mainList = { next = m_mainList, value = "Wait" }
	
	addButtonToGUI(15, 200, "Move")
	addButtonToGUI(15, 215, "Action")
	addButtonToGUI(15, 230, "Wait")
	]]--
end

function OnUpdate()
	if string.match("Normal", getStateAsString()) then
		while m_mainList do
			updateButton(m_mainList.value)
			m_mainList = m_mainList.next
		end
	end
end

function OnDraw()
	if string.match("Normal", getStateAsString()) then
		while m_mainList do
			updateButton(m_mainList.value)
			m_mainList = m_mainList.next
		end
	end
end
