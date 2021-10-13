import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { AuthInterceptorService } from './auth/auth-interceptor.service';
import { AuthComponent } from './auth/auth.component';
import { LoginComponent } from './auth/login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RegisterComponent } from './auth/register/register.component';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { AppRoutingModule } from './app-routing.module';
import { LoadingSpinnerComponent } from './shared/loading-spinner/loading-spinner.component';
import { SearchBarComponent } from './components/search-bar/search-bar.component';
import { SneakersComponent } from './sneakers/sneakers.component';
import { BannerComponent } from './shared/banner/banner.component';
import { FiltersComponent } from './shared/filters/filters.component';
import { ItemCardComponent } from './shared/items/item-card/item-card.component';
import { PaginationComponent } from './shared/pagination/pagination.component';
import { StreetwearComponent } from './streetwear/streetwear.component';
import { ItemsListComponent } from './shared/items/items-list/items-list.component';
import { ItemsComponent } from './shared/items/items.component';
import { ElectronicsComponent } from './electronics/electronics.component';
import { CollectiblesComponent } from './collectibles/collectibles.component';
import { ItemDetailsComponent } from './shared/items/item-details/item-details.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ClipboardModule } from 'ngx-clipboard';
import { ToastrModule } from 'ngx-toastr';
import { SizeModalComponent } from './shared/size-modal/size-modal.component';
import { CategoryCardComponent } from './landing-page/category-card/category-card.component';

@NgModule({
  declarations: [
    AppComponent,
    PageNotFoundComponent,
    AuthComponent,
    LoginComponent,
    RegisterComponent,
    LandingPageComponent,
    LoadingSpinnerComponent,
    ItemsListComponent,
    SneakersComponent,
    BannerComponent,
    FiltersComponent,
    ItemCardComponent,
    StreetwearComponent,
    ItemsComponent,
    ElectronicsComponent,
    CollectiblesComponent,
    ItemDetailsComponent,
    PaginationComponent,
    SearchBarComponent,
    SizeModalComponent,
    CategoryCardComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    ClipboardModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot()
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
