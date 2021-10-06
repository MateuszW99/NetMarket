import { FormGroup } from '@angular/forms';

export class CustomValidators {
  static samePasswords(group: FormGroup): { [s: string]: boolean } {
    const password = group.get('password').value;
    const confirmPassword = group.get('confirmPassword').value;

    return password === confirmPassword ? null : { notSame: true };
  }
}
