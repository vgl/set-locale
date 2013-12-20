Feature Update Key Translation



Scenario: List TranslationKeys

	Given user logged in
	And user is in "Translator" role

	When user clikcs "Keys" buttons

	Then screen lists translation keys descending ordering by last modification date




Scenario:  View a Key

	Given user logged in
	And user is in "Translator" role

	When user clicks /UpdateKey

	Then system binds language combo with last modified 
	And  system binds translation key
	And  system binds description
	And  system binds value




Scenario: Update

	Given user logged in
	And user is in "Translator" role

	When user views /key/edit/{key_name}
	And  key, value, and language have value

	Then save data to db
