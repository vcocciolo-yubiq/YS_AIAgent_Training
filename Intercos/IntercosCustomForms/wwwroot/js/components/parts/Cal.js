
// Access class properties via this.item.PropertyName
const Intercos_Cal = defineComponent({
  extends: FormLayoutItemComponent,
  template: /*html*/`<div>hello this is Intercos_Cal</div>`,
  // data() { return {} },
  // methods: {},
  // created() {
  //     console.log("Intercos_Cal created", this.item);
  // },
  mounted() {
    console.log("Intercos_Cal mounted", this.item);
  },
});
