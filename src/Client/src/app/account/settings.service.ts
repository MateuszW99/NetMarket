import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApiPaths } from '../shared/api-paths';
import { UserSettings } from './user-settings.model';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {
  userSettingsChanged = new Subject<UserSettings>();
  userSellerLevelChanged = new Subject<string>();
  errorCatched = new Subject<string>();
  loading = new Subject<boolean>();

  constructor(private http: HttpClient) {}

  getUserSettings(): void {
    this.loading.next(true);
    this.fetchUserSettings().subscribe(
      (settings: UserSettings) => {
        this.userSettingsChanged.next(settings);
        this.loading.next(false);
      },
      () => {
        this.errorCatched.next('An error occurred while loading the user settings');
        this.loading.next(false);
      }
    );
  }

  getUserSellerLevel(): void {
    this.loading.next(true);
    this.fetchUserSellerLevel().subscribe(
      (userSellerLevel: string) => {
        if (userSellerLevel !== '') {
          this.userSellerLevelChanged.next(userSellerLevel);
        }
        this.loading.next(false);
      },
        () => {
          this.errorCatched.next('An error occurred while loading user seller level');
          this.loading.next(false);
        }
    );
  }

  updateUserSettings(userSettings: UserSettings): Observable<unknown> {
    return this.http.put<unknown>(
      environment.apiUrl + ApiPaths.UserSettings,
      userSettings
    );
  }

  private fetchUserSettings(): Observable<UserSettings> {
    return this.http.get<UserSettings>(environment.apiUrl + ApiPaths.UserSettings);
  }

  private fetchUserSellerLevel(): Observable<string> {
    const requestOptions: Object = {
      responseType: 'text'
    };
    return this.http.get<string>(environment.apiUrl + ApiPaths.UserSettings + '/level', requestOptions);
  }
}
