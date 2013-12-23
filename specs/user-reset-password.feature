Feature: User Management
  Scenario: Reset Password 


    When user views "/user/newpassword" 
            And must fill "EMail"
            And clicks "Send Reset Password Link" button
            And checks if this "EMail" has an account


    Then new password link should be send to this "EMail"