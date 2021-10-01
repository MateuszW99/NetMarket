import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { AuthInterceptorService } from './auth/auth-interceptor.service';
import { AuthComponent } from './auth/auth.component';
import { LoginComponent } from './auth/login/login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { RegisterComponent } from './auth/register/register.component';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { AppRoutingModule } from './app-routing.module';
import { LoadingSpinnerComponent } from './shared/loading-spinner/loading-spinner.component';
import { SearchBarComponent } from './components/search-bar/search-bar.component';
import { SneakersComponent } from './sneakers/sneakers.component';
import { BannerComponent } from './shared/banner/banner.component';
import { FiltersComponent } from './shared/filters/filters.component';
import { ItemsComponent } from './shared/items/items.component';
import { ItemCardComponent } from './shared/items/item-card/item-card.component';
import { PaginationComponent } from './shared/pagination/pagination.component';

@NgModule({
  declarations: [
    AppComponent,
    PageNotFoundComponent,
    AuthComponent,
    LoginComponent,
    RegisterComponent,
    LandingPageComponent,
    LoadingSpinnerComponent,
    SneakersComponent,
    BannerComponent,
    FiltersComponent,
    ItemsComponent,
    ItemCardComponent,
    PaginationComponent,
    SearchBarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptorService,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
