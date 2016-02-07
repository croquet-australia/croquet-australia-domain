Feature: EventSerializerRowKeyGenerator

Scenario Outline: Generate unique & ordered row key
	Given new EventSerializerRowKeyGenerator(<ticks>, <guid>)
	When I call EventSerializerRowKeyGenerator.GenerateRowKey()
	Then the result should be <expectedResult>

    Examples:
    | ticks               | guid                                 | expectedResult                                           |
    | 1                   | 2b51aff9-69bd-4a9a-95e3-bac5115caa3f | 0000000000000000001-2b51aff9-69bd-4a9a-95e3-bac5115caa3f |
    | 1234567890123456789 | 36bcd3f7-2ef7-43d6-9071-f3f6df507d16 | 1234567890123456789-36bcd3f7-2ef7-43d6-9071-f3f6df507d16 |
    | 3155378975999999999 | dc83b9a4-1fca-46e6-adcb-135c2c8c6079 | 3155378975999999999-dc83b9a4-1fca-46e6-adcb-135c2c8c6079 |