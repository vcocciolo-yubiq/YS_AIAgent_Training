
// Access class instance properties via this.item.PropertyName or {{item.PropertyName}}
const Intercos_newField = defineComponent({
  extends: BaseField,
  template: /*html*/`
            <div :class="cssClass"
                v-show="isVisible"
            >
                Hello this is {{item.Title ?? 'Intercos_newField'}}!
            </div>
        `,
  // data() { return {} },
  // methods: {},
  // computed: {},
  // created() {
  //     console.debug("Intercos_newField created", this.item);
  // },
  mounted() {
    console.debug("Intercos_newField mounted", this.item);
  },
});
