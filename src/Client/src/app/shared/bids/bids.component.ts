import { Component, OnInit } from '@angular/core';
import { ItemDetails } from "../items/item-details/item-details.model";
import { Size } from "../size.model";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { BidsService } from "./bids.service";
import { ToastrService } from "ngx-toastr";
import { Router } from "@angular/router";

@Component({
  selector: 'app-bids',
  templateUrl: './bids.component.html',
  styleUrls: ['./bids.component.css']
})
export class BidsComponent implements OnInit {
  itemDetails: ItemDetails;
  size: Size;
  form: FormGroup;
  userWantsToPlaceBid: boolean;

  constructor(
    private bidsService: BidsService,
    private toastrService: ToastrService,
    private router: Router) { }

  ngOnInit(): void {
    // TODO: act in case data.X is empty/null
    this.itemDetails = history.state.data.item;
    this.size = history.state.data.size;

    this.form = new FormGroup({
      item: new FormControl(this.itemDetails.item.id, Validators.nullValidator),
      size: new FormControl(this.size, Validators.nullValidator),
      price: new FormControl('', [ Validators.required, Validators.pattern('^[0-9]+(.[0-9]{0,2})?$')]), // TODO: offer new highestBid
    });

    this.userWantsToPlaceBid = this.form.get('price').value >= this.itemDetails.lowestAsk.price;
  }

  getLabel() {
    return this.userWantsToPlaceBid ? 'Place Bid' : 'Buy';
  }

  onSubmitForm(): void {
    if (this.userWantsToPlaceBid) {
      this.onPlaceBid();
    }
    else {
      this.onBuyNow();
    }
  }

  onPlaceBid(): void {
    this.bidsService.placeBid(this.form.value.item, this.form.value.size, this.form.value.price.toString())
      .subscribe(() => {
        this.router.navigate([`/items/${this.itemDetails.item.id}`])
          .then(() => this.toastrService.success('Bid placed!'));
      });
  }

  onBuyNow(): void {

  }

}
